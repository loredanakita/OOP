namespace loriks3
{
    // Simple dialog to edit all fields of a cosmetic product
    public class EditDialog : Form
    {
        private readonly Dictionary<string, TextBox> _fields = new();
        private readonly CosmeticProduct _product;

        public EditDialog(CosmeticProduct product)
        {
            _product = product;
            Text = $"Edit {product.GetType().Name}";
            Size = new Size(340, 80);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;

            var panel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                Padding = new Padding(8),
                AutoSize = true
            };
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            // Build one row per editable field dynamically
            var fieldValues = product.GetFieldValues();
            foreach (var field in product.GetEditableFields())
            {
                panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));
                panel.Controls.Add(new Label { Text = field + ":", Anchor = AnchorStyles.Left | AnchorStyles.Right, TextAlign = System.Drawing.ContentAlignment.MiddleRight });
                var tb = new TextBox { Text = fieldValues.GetValueOrDefault(field, ""), Dock = DockStyle.Fill };
                panel.Controls.Add(tb);
                _fields[field] = tb;
            }

            // OK / Cancel buttons row
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
            var btnOk = new Button { Text = "OK", DialogResult = DialogResult.OK, Width = 70 };
            var btnCancel = new Button { Text = "Cancel", DialogResult = DialogResult.Cancel, Width = 70 };
            var btnPanel = new FlowLayoutPanel { FlowDirection = FlowDirection.RightToLeft, Dock = DockStyle.Fill };
            btnPanel.Controls.Add(btnCancel);
            btnPanel.Controls.Add(btnOk);
            panel.SetColumnSpan(btnPanel, 2);
            panel.Controls.Add(btnPanel);

            Controls.Add(panel);
            AutoSize = true;
            AcceptButton = btnOk;
            CancelButton = btnCancel;
        }

        // Apply entered values back to the product on OK
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            if (DialogResult == DialogResult.OK)
            {
                var values = _fields.ToDictionary(kv => kv.Key, kv => kv.Value.Text);
                _product.SetDetails(values);
            }
        }
    }
}
