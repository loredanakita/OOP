namespace loriks3
{
    public partial class Form1 : Form
    {
        // ── Data ──────────────────────────────────────────────────────────────
        private readonly List<CosmeticProduct> _products = new();

        // Factory map: type name → creator func (no if-else, no reflection)
        private readonly Dictionary<string, Func<CosmeticProduct>> _factories = new()
        {
            ["Lipstick"]   = () => new Lipstick   { Name = "New Lipstick",   Brand = "Brand", Price = 0, Color = "Red",   Finish = "Matte",  Texture = "Creamy" },
            ["Foundation"] = () => new Foundation { Name = "New Foundation", Brand = "Brand", Price = 0, Color = "Beige", Coverage = "Medium", Shade = "N10" },
            ["Mascara"]    = () => new Mascara    { Name = "New Mascara",    Brand = "Brand", Price = 0, Color = "Black", IsWaterproof = false, Effect = "Volume" },
            ["Eyeshadow"]  = () => new Eyeshadow  { Name = "New Eyeshadow",  Brand = "Brand", Price = 0, Color = "Brown", IsWaterproof = false, PanCount = 12 },
            ["Perfume"]    = () => new Perfume    { Name = "New Perfume",    Brand = "Brand", Price = 0, Color = "Clear", Notes = "Floral", VolumeML = 50 },
            ["Blush"]      = () => new Blush      { Name = "New Blush",      Brand = "Brand", Price = 0, Color = "Pink",  Finish = "Satin", Formula = "Powder" },
        };

        // Discovered storage plugins (ICosmeticPlugin handled in PluginLoader)
        private readonly List<loriks3.PluginContracts.IStoragePlugin> _storagePlugins = new();

        // Extra CLR types contributed by ICosmeticPlugin assemblies (for XmlSerializer)
        private readonly List<Type> _pluginProductTypes = new();

        public Form1()
        {
            InitializeComponent();
            BuildSettingsMenu();
            LoadCosmeticPlugins();
            LoadStoragePlugins();

            // Populate type combo
            comboBoxType.Items.Clear();
            comboBoxType.Items.AddRange(_factories.Keys.ToArray<object>());
            comboBoxType.SelectedIndex = 0;
        }

        // ── Plugin loading ────────────────────────────────────────────────────

        /// <summary>Discovers ICosmeticPlugin instances and registers their factories.</summary>
        private void LoadCosmeticPlugins()
        {
            foreach (var plugin in PluginLoader.LoadAll())
            {
                if (_factories.ContainsKey(plugin.TypeName)) continue;
                var captured = plugin;
                _factories[plugin.TypeName] = () => captured.CreateDefault();

                // Remember the product type for XmlSerializer known-types
                _pluginProductTypes.Add(captured.CreateDefault().GetType());
            }
        }

        /// <summary>Discovers IStoragePlugin instances from the Plugins folder.</summary>
        private void LoadStoragePlugins()
        {
            _storagePlugins.Clear();
            _storagePlugins.AddRange(PluginLoader.LoadStoragePlugins());
            SetInfo($"Storage plugins loaded: {_storagePlugins.Count}");
        }

        // ── Settings menu ─────────────────────────────────────────────────────

        /// <summary>
        /// Builds a "Settings" top-level menu item with sub-items:
        ///   • Plugin Settings — opens PluginSettingsDialog
        ///   • Reload Plugins  — re-scans the Plugins folder at runtime
        /// </summary>
        private void BuildSettingsMenu()
        {
            var menuBar = new MenuStrip();

            var settingsMenu = new ToolStripMenuItem("Settings");

            // Plugin Settings sub-item
            var pluginSettingsItem = new ToolStripMenuItem("Plugin Settings…");
            pluginSettingsItem.Click += (_, _) => OpenPluginSettings();

            // Reload Plugins sub-item
            var reloadItem = new ToolStripMenuItem("Reload Plugins");
            reloadItem.Click += (_, _) =>
            {
                LoadStoragePlugins();
                SetInfo("Plugins reloaded.");
            };

            settingsMenu.DropDownItems.Add(pluginSettingsItem);
            settingsMenu.DropDownItems.Add(reloadItem);
            menuBar.Items.Add(settingsMenu);

            // Insert menu bar at the top of the form
            Controls.Add(menuBar);
            MainMenuStrip = menuBar;
        }

        /// <summary>Opens the Plugin Settings dialog and applies the user's choice.</summary>
        private void OpenPluginSettings()
        {
            using var dlg = new PluginSettingsDialog(
                _storagePlugins,
                XmlStorage.ActivePlugin,
                XmlStorage.PluginSettings);

            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            // Apply selected plugin and its settings globally via XmlStorage
            XmlStorage.ActivePlugin    = dlg.SelectedPlugin;
            XmlStorage.PluginSettings  = dlg.SelectedSettings;

            string pluginName = XmlStorage.ActivePlugin?.Name ?? "(None)";
            SetInfo($"Active storage plugin: {pluginName}");
        }

        // ── EVENT HANDLERS ────────────────────────────────────────────────────

        private void listBoxProducts_DoubleClick(object? sender, EventArgs e) => EditSelected();
        private void btnAdd_Click(object? sender, EventArgs e)    => AddProduct();
        private void btnEdit_Click(object? sender, EventArgs e)   => EditSelected();
        private void btnDelete_Click(object? sender, EventArgs e) => DeleteSelected();
        private void btnSave_Click(object? sender, EventArgs e)   => SaveXml();
        private void btnLoad_Click(object? sender, EventArgs e)   => LoadXml();

        // ── ACTIONS ───────────────────────────────────────────────────────────

        /// <summary>Add a new product of selected type using factory (no if-else).</summary>
        private void AddProduct()
        {
            var typeName = comboBoxType.SelectedItem?.ToString() ?? "";
            var product  = _factories[typeName]();
            using var dlg = new EditDialog(product);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                _products.Add(product);
                RefreshList();
                SetInfo($"Added {product}");
            }
        }

        /// <summary>Edit selected product in-place.</summary>
        private void EditSelected()
        {
            if (listBoxProducts.SelectedIndex < 0) return;
            var product = _products[listBoxProducts.SelectedIndex];
            using var dlg = new EditDialog(product);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                RefreshList();
                SetInfo($"Saved changes to {product}");
            }
        }

        /// <summary>Remove selected product from list.</summary>
        private void DeleteSelected()
        {
            if (listBoxProducts.SelectedIndex < 0) return;
            int idx  = listBoxProducts.SelectedIndex;
            var name = _products[idx].ToString();
            _products.RemoveAt(idx);
            RefreshList();
            SetInfo($"Deleted {name}");
        }

        /// <summary>
        /// Serialize list to disk via SaveFileDialog.
        /// The file extension depends on the active storage plugin.
        /// </summary>
        private void SaveXml()
        {
            // Adjust filter based on active plugin (JSON vs XML)
            bool isJson = XmlStorage.ActivePlugin != null;
            string filter = isJson ? "JSON files|*.json|All files|*.*"
                                   : "XML files|*.xml|All files|*.*";
            string defExt = isJson ? "json" : "xml";

            using var dlg = new SaveFileDialog
            {
                Filter = filter, FileName = "cosmetics." + defExt, DefaultExt = defExt
            };
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            try
            {
                XmlStorage.Save(_products, dlg.FileName, _pluginProductTypes);
                string plugin = XmlStorage.ActivePlugin?.Name ?? "plain XML";
                SetInfo($"Saved {_products.Count} items → {dlg.FileName}  [{plugin}]");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Save error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>Deserialize list from disk via OpenFileDialog.</summary>
        private void LoadXml()
        {
            bool isJson = XmlStorage.ActivePlugin != null;
            string filter = isJson ? "JSON files|*.json|XML files|*.xml|All files|*.*"
                                   : "XML files|*.xml|All files|*.*";

            using var dlg = new OpenFileDialog { Filter = filter };
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            try
            {
                var loaded = XmlStorage.Load(dlg.FileName, _pluginProductTypes);
                _products.Clear();
                _products.AddRange(loaded);
                RefreshList();
                string plugin = XmlStorage.ActivePlugin?.Name ?? "plain XML";
                SetInfo($"Loaded {_products.Count} items ← {dlg.FileName}  [{plugin}]");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Load error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>Rebuild ListBox from current _products list.</summary>
        private void RefreshList()
        {
            listBoxProducts.BeginUpdate();
            listBoxProducts.Items.Clear();
            foreach (var p in _products)
                listBoxProducts.Items.Add(p.ToString());
            listBoxProducts.EndUpdate();
        }

        private void SetInfo(string msg) => labelInfo.Text = msg;

        private void labelType_Click(object sender, EventArgs e) { }
        private void listBoxProducts_SelectedIndexChanged(object sender, EventArgs e) { }
    }
}
