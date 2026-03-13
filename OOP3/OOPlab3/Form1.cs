namespace loriks3
{
    public partial class Form1 : Form
    {
        // In-memory product list
        private readonly List<CosmeticProduct> _products = new();

        // Factory map: type name → creator func (no if-else, no reflection)
        private readonly Dictionary<string, Func<CosmeticProduct>> _factories = new()
        {
            ["Lipstick"] = () => new Lipstick { Name = "New Lipstick", Brand = "Brand", Price = 0, Color = "Red", Finish = "Matte", Texture = "Creamy" },
            ["Foundation"] = () => new Foundation { Name = "New Foundation", Brand = "Brand", Price = 0, Color = "Beige", Coverage = "Medium", Shade = "N10" },
            ["Mascara"] = () => new Mascara { Name = "New Mascara", Brand = "Brand", Price = 0, Color = "Black", IsWaterproof = false, Effect = "Volume" },
            ["Eyeshadow"] = () => new Eyeshadow { Name = "New Eyeshadow", Brand = "Brand", Price = 0, Color = "Brown", IsWaterproof = false, PanCount = 12 },
            ["Perfume"] = () => new Perfume { Name = "New Perfume", Brand = "Brand", Price = 0, Color = "Clear", Notes = "Floral", VolumeML = 50 },
            ["Blush"] = () => new Blush { Name = "New Blush", Brand = "Brand", Price = 0, Color = "Pink", Finish = "Satin", Formula = "Powder" },
        };

        public Form1()
        {
            InitializeComponent();

            // Populate combo from factory keys after designer init
            comboBoxType.Items.AddRange(_factories.Keys.ToArray<object>());
            comboBoxType.SelectedIndex = 0;
        }

        // ── EVENT HANDLERS (named so Designer.cs can reference them) ─────────────

        private void listBoxProducts_DoubleClick(object? sender, EventArgs e) => EditSelected();
        private void btnAdd_Click(object? sender, EventArgs e) => AddProduct();
        private void btnEdit_Click(object? sender, EventArgs e) => EditSelected();
        private void btnDelete_Click(object? sender, EventArgs e) => DeleteSelected();
        private void btnSave_Click(object? sender, EventArgs e) => SaveXml();
        private void btnLoad_Click(object? sender, EventArgs e) => LoadXml();

        // ── ACTIONS ───────────────────────────────────────────────────────────────

        // Add a new product of selected type using factory (no if-else)
        private void AddProduct()
        {
            var typeName = comboBoxType.SelectedItem?.ToString() ?? "";
            var product = _factories[typeName]();
            using var dlg = new EditDialog(product);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                _products.Add(product);
                RefreshList();
                SetInfo($"Added {product}");
            }
        }

        // Edit selected product in-place
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

        // Remove selected product from list
        private void DeleteSelected()
        {
            if (listBoxProducts.SelectedIndex < 0) return;
            var idx = listBoxProducts.SelectedIndex;
            var name = _products[idx].ToString();
            _products.RemoveAt(idx);
            RefreshList();
            SetInfo($"Deleted {name}");
        }

        // Serialize list to XML via SaveFileDialog
        private void SaveXml()
        {
            using var dlg = new SaveFileDialog { Filter = "XML files|*.xml", FileName = "cosmetics.xml" };
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                XmlStorage.Save(_products, dlg.FileName);
                SetInfo($"Saved {_products.Count} items to {dlg.FileName}");
            }
        }

        // Deserialize list from XML via OpenFileDialog
        private void LoadXml()
        {
            using var dlg = new OpenFileDialog { Filter = "XML files|*.xml" };
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                var loaded = XmlStorage.Load(dlg.FileName);
                _products.Clear();
                _products.AddRange(loaded);
                RefreshList();
                SetInfo($"Loaded {_products.Count} items from {dlg.FileName}");
            }
        }

        // Rebuild ListBox from current _products list
        private void RefreshList()
        {
            listBoxProducts.BeginUpdate();
            listBoxProducts.Items.Clear();
            foreach (var p in _products)
                listBoxProducts.Items.Add(p.ToString());
            listBoxProducts.EndUpdate();
        }

        private void SetInfo(string msg) => labelInfo.Text = msg;

        private void labelType_Click(object sender, EventArgs e)
        {

        }

        private void listBoxProducts_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
