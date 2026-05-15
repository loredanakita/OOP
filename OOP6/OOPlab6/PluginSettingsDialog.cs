using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private readonly TextBox _txtSettings = new()
        {
            Dock = DockStyle.Fill,
            Multiline = true,
            ScrollBars = ScrollBars.Vertical,
            Font = new System.Drawing.Font("Consolas", 9f)
        };
        private readonly Label _lblHint = new()
        {
            Text = "Settings (one key=value per line, multi-line values separated by ~~~):",
            Dock = DockStyle.Fill,
            TextAlign = System.Drawing.ContentAlignment.BottomLeft
        };
        private readonly Button _btnOk = new() { Text = "OK", DialogResult = DialogResult.OK, Width = 80 };
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
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft
            };
            btnPanel.Controls.Add(_btnCancel);
            btnPanel.Controls.Add(_btnOk);
            _btnOk.Click += BtnOk_Click;

            tbl.Controls.Add(_cmbPlugin, 0, 0);
            tbl.Controls.Add(_lblHint, 0, 1);
            tbl.Controls.Add(_txtSettings, 0, 2);
            tbl.Controls.Add(btnPanel, 0, 3);

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
            SelectedPlugin = _plugins[_cmbPlugin.SelectedIndex];
            SelectedSettings = ParseSettings(_txtSettings.Text);
            DialogResult = DialogResult.OK;
            Close();
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        /// <summary>
        /// Converts settings dictionary to text for display in the TextBox.
        /// Multi-line values (e.g. XSLT stylesheets) are preserved verbatim.
        /// Entries are separated by a "~~~" line so ParseSettings can reconstruct them.
        /// Format:
        ///   key=first line of value
        ///   continuation line 2
        ///   ~~~
        ///   key2=single line value
        /// </summary>
        private static string SettingsToText(Dictionary<string, string> d)
        {
            var sb = new StringBuilder();
            bool first = true;
            foreach (var kv in d)
            {
                if (!first) sb.AppendLine("~~~");   // separator between entries
                first = false;

                // Write "key=value"; value may contain newlines (e.g. XSLT stylesheet)
                sb.Append(kv.Key);
                sb.Append('=');
                sb.Append(kv.Value);
                sb.AppendLine();
            }
            return sb.ToString().TrimEnd();
        }

        /// <summary>
        /// Parses the multi-line TextBox content back into a settings dictionary.
        /// Entries are separated by "~~~" lines.
        /// Within each entry the first line is "key=firstValueLine";
        /// subsequent lines are appended to that entry's value,
        /// preserving the original multi-line content (e.g. XSLT stylesheets).
        /// </summary>
        private static Dictionary<string, string> ParseSettings(string text)
        {
            var result = new Dictionary<string, string>();
            if (string.IsNullOrWhiteSpace(text)) return result;

            // Split the whole text into per-entry blocks on the "~~~" separator
            var blocks = text.Split(new[] { "~~~" }, StringSplitOptions.None);

            foreach (var block in blocks)
            {
                // Remove only the leading/trailing blank lines around the block
                string trimmedBlock = block.Trim('\r', '\n');
                if (string.IsNullOrWhiteSpace(trimmedBlock)) continue;

                // The first '=' separates the key from everything else (including newlines)
                int eq = trimmedBlock.IndexOf('=');
                if (eq <= 0) continue;

                string key = trimmedBlock[..eq].Trim();
                string value = trimmedBlock[(eq + 1)..];  // everything after '=', newlines intact

                // Normalise line endings (TextBox on Windows uses \r\n)
                value = value.Replace("\r\n", "\n").Replace("\r", "\n");

                // Strip only the single newline immediately after '=' (artefact of AppendLine)
                if (value.StartsWith('\n')) value = value[1..];

                if (!string.IsNullOrEmpty(key))
                    result[key] = value;
            }
            return result;
        }
    }
}