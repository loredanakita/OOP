namespace loriks3
{
    public partial class Form1 : Form
    {
        // ── Singleton: single authoritative product list ───────────────────────
        // Instead of a local List<CosmeticProduct>, we use the shared repository.
        // All mutations go through ProductRepository so observers are notified.
        private readonly ProductRepository _repo = ProductRepository.Instance;

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

        // Discovered storage plugins
        private readonly List<loriks3.PluginContracts.IStoragePlugin> _storagePlugins = new();

        // Extra CLR types contributed by ICosmeticPlugin assemblies (for XmlSerializer)
        private readonly List<Type> _pluginProductTypes = new();

        public Form1()
        {
            InitializeComponent();
            BuildSettingsMenu();
            LoadCosmeticPlugins();
            LoadStoragePlugins();

            // Observer: subscribe the status label to repository changes
            _repo.Subscribe(new StatusBarObserver(labelInfo));

            // Observer: subscribe the ListBox refresh to repository changes
            _repo.Subscribe(new ListBoxRefreshObserver(listBoxProducts, _repo));

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
                _pluginProductTypes.Add(captured.CreateDefault().GetType());
            }
        }

        /// <summary>Discovers IStoragePlugin instances from the Plugins folder.</summary>
        private void LoadStoragePlugins()
        {
            _storagePlugins.Clear();
            _storagePlugins.AddRange(PluginLoader.LoadStoragePlugins());
            labelInfo.Text = $"Storage plugins loaded: {_storagePlugins.Count}";
        }

        // ── Settings menu ─────────────────────────────────────────────────────

        /// <summary>Builds Settings menu with Plugin Settings and Reload Plugins.</summary>
        private void BuildSettingsMenu()
        {
            var menuBar      = new MenuStrip();
            var settingsMenu = new ToolStripMenuItem("Settings");

            var pluginSettingsItem = new ToolStripMenuItem("Plugin Settings…");
            pluginSettingsItem.Click += (_, _) => OpenPluginSettings();

            var reloadItem = new ToolStripMenuItem("Reload Plugins");
            reloadItem.Click += (_, _) =>
            {
                LoadStoragePlugins();
                labelInfo.Text = "Plugins reloaded.";
            };

            settingsMenu.DropDownItems.Add(pluginSettingsItem);
            settingsMenu.DropDownItems.Add(reloadItem);
            menuBar.Items.Add(settingsMenu);
            Controls.Add(menuBar);
            MainMenuStrip = menuBar;
        }

        /// <summary>Opens Plugin Settings dialog and applies the user's choice.</summary>
        private void OpenPluginSettings()
        {
            using var dlg = new PluginSettingsDialog(
                _storagePlugins,
                XmlStorage.ActivePlugin,
                XmlStorage.PluginSettings);

            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            XmlStorage.ActivePlugin   = dlg.SelectedPlugin;
            XmlStorage.PluginSettings = dlg.SelectedSettings;
            labelInfo.Text = $"Active storage plugin: {XmlStorage.ActivePlugin?.Name ?? "(None)"}";
        }

        // ── EVENT HANDLERS ────────────────────────────────────────────────────

        private void listBoxProducts_DoubleClick(object? sender, EventArgs e) => EditSelected();
        private void btnAdd_Click(object? sender, EventArgs e)    => AddProduct();
        private void btnEdit_Click(object? sender, EventArgs e)   => EditSelected();
        private void btnDelete_Click(object? sender, EventArgs e) => DeleteSelected();
        private void btnSave_Click(object? sender, EventArgs e)   => SaveXml();
        private void btnLoad_Click(object? sender, EventArgs e)   => LoadXml();

        // ── ACTIONS ───────────────────────────────────────────────────────────

        /// <summary>Add a new product of selected type using factory.</summary>
        private void AddProduct()
        {
            var typeName = comboBoxType.SelectedItem?.ToString() ?? "";
            var product  = _factories[typeName]();
            using var dlg = new EditDialog(product);
            if (dlg.ShowDialog(this) == DialogResult.OK)
                _repo.Add(product);   // Observer notified automatically
        }

        /// <summary>Edit selected product in-place.</summary>
        private void EditSelected()
        {
            if (listBoxProducts.SelectedIndex < 0) return;
            int idx     = listBoxProducts.SelectedIndex;
            var product = _repo.Products[idx];
            using var dlg = new EditDialog(product);
            if (dlg.ShowDialog(this) == DialogResult.OK)
                _repo.NotifyEdited(idx);  // Observer notified automatically
        }

        /// <summary>Remove selected product from list.</summary>
        private void DeleteSelected()
        {
            if (listBoxProducts.SelectedIndex < 0) return;
            _repo.RemoveAt(listBoxProducts.SelectedIndex); // Observer notified
        }

        /// <summary>Serialize list to disk via SaveFileDialog.</summary>
        private void SaveXml()
        {
            bool isJson = XmlStorage.ActivePlugin != null;
            string filter = isJson ? "JSON files|*.json|XML files|*.xml|All files|*.*"
                                   : "XML files|*.xml|All files|*.*";
            string defExt = isJson ? "json" : "xml";

            using var dlg = new SaveFileDialog
            {
                Filter = filter, FileName = "cosmetics." + defExt, DefaultExt = defExt
            };
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            try
            {
                XmlStorage.Save(_repo.Products, dlg.FileName, _pluginProductTypes);
                labelInfo.Text = $"Saved {_repo.Products.Count} items → {dlg.FileName}" +
                                 $"  [{XmlStorage.ActivePlugin?.Name ?? "plain XML"}]";
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
                _repo.ReplaceAll(loaded);  // Observer notified automatically
                labelInfo.Text = $"Loaded {_repo.Products.Count} items ← {dlg.FileName}" +
                                 $"  [{XmlStorage.ActivePlugin?.Name ?? "plain XML"}]";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Load error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void labelType_Click(object sender, EventArgs e) { }
        private void listBoxProducts_SelectedIndexChanged(object sender, EventArgs e) { }
    }

    // ── Second concrete Observer: keeps the ListBox in sync ───────────────────

    /// <summary>
    /// Concrete Observer: refreshes the ListBox whenever the product list changes.
    /// Registered in Form1 constructor alongside StatusBarObserver.
    /// </summary>
    internal sealed class ListBoxRefreshObserver : IProductObserver
    {
        private readonly System.Windows.Forms.ListBox _listBox;
        private readonly ProductRepository            _repo;

        public ListBoxRefreshObserver(System.Windows.Forms.ListBox listBox,
                                      ProductRepository repo)
        {
            _listBox = listBox;
            _repo    = repo;
        }

        /// <inheritdoc/>
        public void OnProductChanged(object sender, ProductChangedEventArgs e)
        {
            // Rebuild the ListBox from the current repository snapshot
            _listBox.BeginUpdate();
            _listBox.Items.Clear();
            foreach (var p in _repo.Products)
                _listBox.Items.Add(p.ToString());
            _listBox.EndUpdate();
        }
    }
}
