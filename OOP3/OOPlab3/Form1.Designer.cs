namespace loriks3
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        // All controls declared as fields so VS designer can see and move them
        private ListBox listBoxProducts;
        private ComboBox comboBoxType;
        private Label labelType;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnSave;
        private Button btnLoad;
        private Label labelInfo;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            listBoxProducts = new ListBox();
            comboBoxType = new ComboBox();
            labelType = new Label();
            btnAdd = new Button();
            btnEdit = new Button();
            btnDelete = new Button();
            btnSave = new Button();
            btnLoad = new Button();
            labelInfo = new Label();
            SuspendLayout();
            // 
            // listBoxProducts
            // 
            listBoxProducts.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            listBoxProducts.Font = new Font("Consolas", 9F);
            listBoxProducts.Location = new Point(15, 15);
            listBoxProducts.Margin = new Padding(4);
            listBoxProducts.Name = "listBoxProducts";
            listBoxProducts.Size = new Size(699, 400);
            listBoxProducts.TabIndex = 0;
            listBoxProducts.SelectedIndexChanged += listBoxProducts_SelectedIndexChanged;
            listBoxProducts.DoubleClick += listBoxProducts_DoubleClick;
            // 
            // comboBoxType
            // 
            comboBoxType.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            comboBoxType.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxType.Location = new Point(68, 442);
            comboBoxType.Margin = new Padding(4);
            comboBoxType.Name = "comboBoxType";
            comboBoxType.Size = new Size(149, 33);
            comboBoxType.TabIndex = 2;
            // 
            // labelType
            // 
            labelType.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            labelType.Location = new Point(-2, 430);
            labelType.Margin = new Padding(4, 0, 4, 0);
            labelType.Name = "labelType";
            labelType.Size = new Size(78, 59);
            labelType.TabIndex = 1;
            labelType.Text = "Type:";
            labelType.TextAlign = ContentAlignment.MiddleLeft;
            labelType.Click += labelType_Click;
            // 
            // btnAdd
            // 
            btnAdd.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnAdd.Location = new Point(230, 441);
            btnAdd.Margin = new Padding(4);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(88, 32);
            btnAdd.TabIndex = 3;
            btnAdd.Text = "Add";
            btnAdd.Click += btnAdd_Click;
            // 
            // btnEdit
            // 
            btnEdit.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnEdit.Location = new Point(325, 441);
            btnEdit.Margin = new Padding(4);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(88, 32);
            btnEdit.TabIndex = 4;
            btnEdit.Text = "Edit";
            btnEdit.Click += btnEdit_Click;
            // 
            // btnDelete
            // 
            btnDelete.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnDelete.Location = new Point(420, 441);
            btnDelete.Margin = new Padding(4);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(88, 32);
            btnDelete.TabIndex = 5;
            btnDelete.Text = "Delete";
            btnDelete.Click += btnDelete_Click;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnSave.Location = new Point(525, 441);
            btnSave.Margin = new Padding(4);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(100, 32);
            btnSave.TabIndex = 6;
            btnSave.Text = "Save XML";
            btnSave.Click += btnSave_Click;
            // 
            // btnLoad
            // 
            btnLoad.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnLoad.Location = new Point(632, 441);
            btnLoad.Margin = new Padding(4);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(100, 32);
            btnLoad.TabIndex = 7;
            btnLoad.Text = "Load XML";
            btnLoad.Click += btnLoad_Click;
            // 
            // labelInfo
            // 
            labelInfo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            labelInfo.Font = new Font("Segoe UI", 8F);
            labelInfo.ForeColor = Color.Gray;
            labelInfo.Location = new Point(15, 485);
            labelInfo.Margin = new Padding(4, 0, 4, 0);
            labelInfo.Name = "labelInfo";
            labelInfo.Size = new Size(700, 22);
            labelInfo.TabIndex = 8;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Pink;
            ClientSize = new Size(745, 520);
            Controls.Add(listBoxProducts);
            Controls.Add(labelType);
            Controls.Add(comboBoxType);
            Controls.Add(btnAdd);
            Controls.Add(btnEdit);
            Controls.Add(btnDelete);
            Controls.Add(btnSave);
            Controls.Add(btnLoad);
            Controls.Add(labelInfo);
            Margin = new Padding(4);
            MinimumSize = new Size(620, 461);
            Name = "Form1";
            Text = "Cosmetics Catalog";
            ResumeLayout(false);
        }
    }
}
