using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using loriks3.PluginContracts;

namespace loriks3
{
    /// <summary>
    /// Modal dialog for selecting and configuring an IStoragePlugin.
    /// Shown from the Settings menu in Form1.
    /// Allows the user to:
    ///   1. Pick a storage plugin (or None)
    ///   2. Edit the plugin's key=value settings
    /// </summary>
    public sealed class PluginSettingsDialog : Form
    {
        // ── Controls ──────────────────────────────────────────────────────────
        private readonly ComboBox _cmbPlugin = new() { Dock = DockStyle.Fill };
        private readonly TextBox  _txtSettings = new()
        {
            Dock = DockStyle.Fill, Multiline = true, ScrollBars = ScrollBars.Vertical,
            Font = new System.Drawing.Font("Consolas", 9f)
        };
        private readonly Label _lblHint = new()
        {
            Text = "Settings (one key=value per line):",
            Dock = DockStyle.Fill, TextAlign = System.Drawing.ContentAlignment.BottomLeft
        };
        private readonly Button _btnOk     = new() { Text = "OK",     DialogResult = DialogResult.OK,     Width = 80 };
        private readonly Button _btnCancel = new() { Text = "Cancel", DialogResult = DialogResult.Cancel, Width = 80 };

        // ── Data ──────────────────────────────────────────────────────────────
        private readonly List<IStoragePlugin?> _plugins; // index 0 = "(None)"

        /// <summary>Plugin chosen by user; null means no plugin.</summary>
        public IStoragePlugin? SelectedPlugin { get; private set; }

        /// <summary>Settings string entered by user, parsed to dictionary.</summary>
        public Dictionary<string, string> SelectedSettings { get; private set; } = new();

        // ── Constructor ───────────────────────────────────────────────────────

        /// <summary>
        /// Initialises the dialog with available storage plugins.
        /// </summary>
        /// <param name="available">All discovered IStoragePlugin instances.</param>
        /// <param name="current">Currently active plugin (may be null).</param>
        /// <param name="currentSettings">Current plugin settings.</param>
        public PluginSettingsDialog(
            IReadOnlyList<IStoragePlugin> available,
            IStoragePlugin? current,
            Dictionary<string, string> currentSettings)
        {
            Text = "Plugin Settings";
            Width = 420; Height = 340;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = MinimizeBox = false;

            // Build plugin list: null entry first = "(None)"
            _plugins = new List<IStoragePlugin?> { null };
            _plugins.AddRange(available);

            // Fill combo
            _cmbPlugin.Items.Add("(None)");
            foreach (var p in available)
                _cmbPlugin.Items.Add($"{p.Name} — {p.Description}");

            // Select current plugin
            int selIdx = current == null ? 0 :
                _plugins.FindIndex(p => p?.Name == current.Name);
            _cmbPlugin.SelectedIndex = selIdx < 0 ? 0 : selIdx;

            // Populate settings text box
            _txtSettings.Text = SettingsToText(currentSettings);

            // When plugin selection changes, reload default settings
            _cmbPlugin.SelectedIndexChanged += (_, _) => OnPluginChanged();

            // Layout
            var tbl = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4,
                Padding = new Padding(8)
            };
            tbl.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            tbl.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            tbl.RowStyles.Add(new RowStyle(SizeType.Absolute, 36));

            var btnPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft
            };
            btnPanel.Controls.Add(_btnCancel);
            btnPanel.Controls.Add(_btnOk);
            _btnOk.Click += BtnOk_Click;

            tbl.Controls.Add(_cmbPlugin, 0, 0);
            tbl.Controls.Add(_lblHint,   0, 1);
            tbl.Controls.Add(_txtSettings, 0, 2);
            tbl.Controls.Add(btnPanel,   0, 3);

            Controls.Add(tbl);
            AcceptButton = _btnOk;
            CancelButton = _btnCancel;
        }

        // ── Event handlers ────────────────────────────────────────────────────

        /// <summary>When the user selects a different plugin, load its default settings.</summary>
        private void OnPluginChanged()
        {
            var plugin = _plugins[_cmbPlugin.SelectedIndex];
            if (plugin != null)
                _txtSettings.Text = SettingsToText(plugin.GetDefaultSettings());
            else
                _txtSettings.Clear();
        }

        /// <summary>Validate and store choices when OK is clicked.</summary>
        private void BtnOk_Click(object? sender, EventArgs e)
        {
            SelectedPlugin   = _plugins[_cmbPlugin.SelectedIndex];
            SelectedSettings = ParseSettings(_txtSettings.Text);
            DialogResult = DialogResult.OK;
            Close();
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        /// <summary>Converts dictionary to multi-line key=value text.</summary>
        private static string SettingsToText(Dictionary<string, string> d) =>
            string.Join(Environment.NewLine, d.Select(kv => $"{kv.Key}={kv.Value}"));

        /// <summary>Parses multi-line key=value text into a dictionary.</summary>
        private static Dictionary<string, string> ParseSettings(string text)
        {
            var result = new Dictionary<string, string>();
            foreach (string line in text.Split('\n'))
            {
                string trimmed = line.Trim();
                int eq = trimmed.IndexOf('=');
                if (eq > 0)
                    result[trimmed[..eq].Trim()] = trimmed[(eq + 1)..].Trim();
            }
            return result;
        }
    }
}
