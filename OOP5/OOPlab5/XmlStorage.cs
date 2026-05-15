using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using loriks3.PluginContracts;

namespace loriks3
{
    /// <summary>
    /// Handles XML serialization/deserialization of the product list.
    /// Supports optional IStoragePlugin hooks that transform the data
    /// before writing to disk and after reading from disk.
    /// </summary>
    public static class XmlStorage
    {
        // ── Known concrete types for XmlSerializer ─────────────────────────────
        // Add new types here when new built-in product classes are introduced.
        private static readonly Type[] _knownTypes =
        {
            typeof(Lipstick), typeof(Foundation), typeof(Mascara),
            typeof(Eyeshadow), typeof(Perfume), typeof(Blush)
        };

        // ── Active storage plugin (null = no transformation) ───────────────────
        /// <summary>Currently selected storage plugin; null means plain XML.</summary>
        public static IStoragePlugin? ActivePlugin { get; set; }

        /// <summary>Settings passed to the active plugin (editable via Plugin Settings dialog).</summary>
        public static Dictionary<string, string> PluginSettings { get; set; } = new();

        // ── Public API ─────────────────────────────────────────────────────────

        /// <summary>
        /// Serializes <paramref name="products"/> to <paramref name="path"/>.
        /// If an IStoragePlugin is active, the raw XML is passed through
        /// <see cref="IStoragePlugin.ProcessBeforeSave"/> before writing.
        /// </summary>
        public static void Save(IEnumerable<CosmeticProduct> products, string path,
                                IEnumerable<Type>? extraTypes = null)
        {
            // Build the full known-types list (built-ins + plugin types)
            Type[] allTypes = BuildKnownTypes(extraTypes);
            var serializer = new XmlSerializer(typeof(List<CosmeticProduct>), allTypes);

            // Serialize to an in-memory XML string first
            string xmlContent;
            using (var sw = new StringWriter())
            {
                using var xw = XmlWriter.Create(sw, new XmlWriterSettings { Indent = true });
                serializer.Serialize(xw, new List<CosmeticProduct>(products));
                xmlContent = sw.ToString();
            }

            // Apply plugin transformation (e.g. XML → JSON) if one is active
            string fileContent = ActivePlugin != null
                ? ActivePlugin.ProcessBeforeSave(xmlContent, PluginSettings)
                : xmlContent;

            File.WriteAllText(path, fileContent, Encoding.UTF8);
        }

        /// <summary>
        /// Loads products from <paramref name="path"/>.
        /// If an IStoragePlugin is active, file content is passed through
        /// <see cref="IStoragePlugin.ProcessAfterLoad"/> before XML deserialization.
        /// </summary>
        public static List<CosmeticProduct> Load(string path,
                                                  IEnumerable<Type>? extraTypes = null)
        {
            string fileContent = File.ReadAllText(path, Encoding.UTF8);

            // Reverse plugin transformation (e.g. JSON → XML) if one is active
            string xmlContent = ActivePlugin != null
                ? ActivePlugin.ProcessAfterLoad(fileContent, PluginSettings)
                : fileContent;

            Type[] allTypes = BuildKnownTypes(extraTypes);
            var serializer = new XmlSerializer(typeof(List<CosmeticProduct>), allTypes);

            using var sr = new StringReader(xmlContent);
            return (List<CosmeticProduct>?)serializer.Deserialize(sr)
                   ?? new List<CosmeticProduct>();
        }

        // ── Helpers ────────────────────────────────────────────────────────────

        /// <summary>Combines built-in known types with any extra types from plugins.</summary>
        private static Type[] BuildKnownTypes(IEnumerable<Type>? extra)
        {
            if (extra == null) return _knownTypes;
            var all = new List<Type>(_knownTypes);
            all.AddRange(extra);
            return all.ToArray();
        }
    }
}
