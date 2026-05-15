namespace loriks3
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private ListBox   listBoxProducts;
        private ComboBox  comboBoxType;
        private Label     labelType;
        private Button    btnAdd;
        private Button    btnEdit;
        private Button    btnDelete;
        private Button    btnSave;
        private Button    btnLoad;
        private Label     labelInfo;
        private MenuStrip mainMenuStrip;
        private Panel     panelBottom;
        private Panel     panelStatus;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            listBoxProducts = new ListBox();
            comboBoxType    = new ComboBox();
            labelType       = new Label();
            btnAdd          = new Button();
            btnEdit         = new Button();
            btnDelete       = new Button();
            btnSave         = new Button();
            btnLoad         = new Button();
            labelInfo       = new Label();
            mainMenuStrip   = new MenuStrip();
            panelBottom     = new Panel();
            panelStatus     = new Panel();

            SuspendLayout();

            // ── MenuStrip ─────────────────────────────────────────────────────
            mainMenuStrip.Dock     = DockStyle.Top;
            mainMenuStrip.Name     = "mainMenuStrip";
            mainMenuStrip.TabIndex = 9;

            // ── panelStatus — status bar at very bottom ───────────────────────
            panelStatus.Dock    = DockStyle.Bottom;
            panelStatus.Height  = 24;
            panelStatus.Padding = new Padding(8, 3, 0, 0);

            labelInfo.Dock      = DockStyle.Fill;
            labelInfo.Font      = new Font("Segoe UI", 8F);
            labelInfo.ForeColor = Color.DimGray;
            labelInfo.Name      = "labelInfo";
            labelInfo.TabIndex  = 8;
            panelStatus.Controls.Add(labelInfo);

            // ── panelBottom — toolbar row above status bar ────────────────────
            panelBottom.Dock    = DockStyle.Bottom;
            panelBottom.Height  = 38;
            panelBottom.Padding = new Padding(4, 4, 4, 4);

            labelType.Location  = new Point(4, 8);
            labelType.Size      = new Size(46, 22);
            labelType.Text      = "Type:";
            labelType.TextAlign = ContentAlignment.MiddleLeft;
            labelType.Name      = "labelType";
            labelType.TabIndex  = 1;

            comboBoxType.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxType.Location      = new Point(54, 8);
            comboBoxType.Size          = new Size(140, 22);
            comboBoxType.Name          = "comboBoxType";
            comboBoxType.TabIndex      = 2;

            btnAdd.Location = new Point(202, 7); btnAdd.Size = new Size(72, 24);
            btnAdd.Text     = "Add";             btnAdd.Name = "btnAdd";
            btnAdd.TabIndex = 3;                 btnAdd.Click += btnAdd_Click;

            btnEdit.Location = new Point(280, 7); btnEdit.Size = new Size(72, 24);
            btnEdit.Text     = "Edit";            btnEdit.Name = "btnEdit";
            btnEdit.TabIndex = 4;                 btnEdit.Click += btnEdit_Click;

            btnDelete.Location = new Point(358, 7); btnDelete.Size = new Size(72, 24);
            btnDelete.Text     = "Delete";          btnDelete.Name = "btnDelete";
            btnDelete.TabIndex = 5;                 btnDelete.Click += btnDelete_Click;

            btnSave.Location = new Point(448, 7); btnSave.Size = new Size(100, 24);
            btnSave.Text     = "Save XML";        btnSave.Name = "btnSave";
            btnSave.TabIndex = 6;                 btnSave.Click += btnSave_Click;

            btnLoad.Location = new Point(554, 7); btnLoad.Size = new Size(100, 24);
            btnLoad.Text     = "Load XML";        btnLoad.Name = "btnLoad";
            btnLoad.TabIndex = 7;                 btnLoad.Click += btnLoad_Click;

            panelBottom.Controls.Add(labelType);
            panelBottom.Controls.Add(comboBoxType);
            panelBottom.Controls.Add(btnAdd);
            panelBottom.Controls.Add(btnEdit);
            panelBottom.Controls.Add(btnDelete);
            panelBottom.Controls.Add(btnSave);
            panelBottom.Controls.Add(btnLoad);

            // ── listBoxProducts — fills all remaining space ───────────────────
            listBoxProducts.Dock     = DockStyle.Fill;
            listBoxProducts.Font     = new Font("Consolas", 9F);
            listBoxProducts.Name     = "listBoxProducts";
            listBoxProducts.TabIndex = 0;
            listBoxProducts.SelectedIndexChanged += listBoxProducts_SelectedIndexChanged;
            listBoxProducts.DoubleClick          += listBoxProducts_DoubleClick;

            // ── Form1 ─────────────────────────────────────────────────────────
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode       = AutoScaleMode.Font;
            BackColor           = Color.Pink;
            ClientSize          = new Size(730, 520);
            MinimumSize         = new Size(600, 420);
            MainMenuStrip       = mainMenuStrip;
            Name                = "Form1";
            Text                = "Cosmetics Catalog";

            // Dock order: Controls.Add последовательность важна!
            // Fill должен быть добавлен ДО Bottom/Top чтобы правильно занять место
            Controls.Add(listBoxProducts);  // DockStyle.Fill
            Controls.Add(panelBottom);      // DockStyle.Bottom
            Controls.Add(panelStatus);      // DockStyle.Bottom
            Controls.Add(mainMenuStrip);    // DockStyle.Top

            ResumeLayout(false);
            PerformLayout();
        }
    }
}
