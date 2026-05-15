using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Xml;
using System.Xml.Linq;
using loriks3.PluginContracts;

namespace Plugin.XmlToJson
{
    // ─────────────────────────────────────────────────────────────────────────
    //  Plugin: XmlToJson
    //  Before save : XML → JSON  (file written as .json)
    //  After load  : JSON → XML  (fed back to XmlSerializer)
    //  Drop Plugin.XmlToJson.dll into <exe>/Plugins/ and restart.
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Converts XML ↔ JSON so that files are stored as JSON on disk
    /// while the host application continues to work with plain XML internally.
    /// Key fix: xsi:type attributes are preserved as special JSON keys ("@xsi:type")
    /// so that XmlSerializer can reconstruct the correct concrete types on load.
    /// </summary>
    public sealed class XmlToJsonPlugin : IStoragePlugin
    {
        // ── IStoragePlugin ────────────────────────────────────────────────────

        /// <inheritdoc/>
        public string Name => "XML → JSON Transform";

        /// <inheritdoc/>
        public string Description =>
            "Transforms XML to JSON before saving and converts JSON back to XML after loading.";

        /// <inheritdoc/>
        public Dictionary<string, string> GetDefaultSettings() => new()
        {
            // Whether to pretty-print the JSON output (true/false)
            ["PrettyPrint"] = "true",
            // Root element name used when rebuilding XML from JSON
            ["XmlRoot"] = "ArrayOfCosmeticProduct",
        };

        // ── ProcessBeforeSave : XML → JSON ────────────────────────────────────

        /// <summary>
        /// Converts the XML string produced by XmlSerializer into JSON.
        /// XML attributes (like xsi:type) are stored as "@attributeName" keys.
        /// </summary>
        public string ProcessBeforeSave(string xmlContent, Dictionary<string, string> settings)
        {
            bool pretty = !settings.TryGetValue("PrettyPrint", out var pp) ||
                          !bool.TryParse(pp, out var b) || b;

            XDocument doc = XDocument.Parse(xmlContent);
            object? jsonObj = XElementToObject(doc.Root!);

            var opts = new JsonSerializerOptions { WriteIndented = pretty };
            return JsonSerializer.Serialize(jsonObj, opts);
        }

        // ── ProcessAfterLoad : JSON → XML ─────────────────────────────────────

        /// <summary>
        /// Converts JSON (written by ProcessBeforeSave) back to XML so that
        /// XmlSerializer can deserialize the products normally.
        /// Restores XML attributes from "@attributeName" JSON keys.
        /// </summary>
        public string ProcessAfterLoad(string fileContent, Dictionary<string, string> settings)
        {
            settings.TryGetValue("XmlRoot", out var rootName);
            rootName ??= "ArrayOfCosmeticProduct";

            using JsonDocument jdoc = JsonDocument.Parse(fileContent);
            XElement root = JsonElementToXElement(rootName, jdoc.RootElement);

            // Build full XML declaration with namespaces that XmlSerializer expects
            var sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-16\"?>");
            sb.Append(root.ToString(SaveOptions.None));
            return sb.ToString();
        }

        // ── XML → JSON helpers ────────────────────────────────────────────────

        /// <summary>
        /// Recursively converts an XElement into a plain object suitable for
        /// JSON serialization. XML attributes are stored as "@name" keys.
        /// </summary>
        private static object? XElementToObject(XElement el)
        {
            bool hasChildren = el.HasElements;
            bool hasAttribs = el.HasAttributes;

            // Pure leaf with no attributes → just return the text value
            if (!hasChildren && !hasAttribs)
                return el.Value;

            var dict = new Dictionary<string, object?>();

            // Store XML attributes as "@localName" so we can restore them later
            foreach (XAttribute attr in el.Attributes())
            {
                // Use full name with prefix for namespaced attrs, e.g. "@xsi:type"
                string attrKey = "@" + (attr.Name.Namespace == XNamespace.None
                    ? attr.Name.LocalName
                    : attr.Name.LocalName);   // simplified: store as @localName
                dict[attrKey] = attr.Value;
            }

            if (!hasChildren)
            {
                // Has attributes but no child elements — store text as "#text"
                if (!string.IsNullOrEmpty(el.Value))
                    dict["#text"] = el.Value;
                return dict;
            }

            // Group children by local name to detect arrays vs single objects
            var groups = new Dictionary<string, List<object?>>();
            foreach (XElement child in el.Elements())
            {
                string key = child.Name.LocalName;
                if (!groups.ContainsKey(key)) groups[key] = new List<object?>();
                groups[key].Add(XElementToObject(child));
            }

            // Flatten single-item groups; keep lists for multiple items
            foreach (var kv in groups)
                dict[kv.Key] = kv.Value.Count == 1 ? kv.Value[0] : (object?)kv.Value;

            return dict;
        }

        // ── JSON → XML helpers ────────────────────────────────────────────────

        // XSI namespace needed by XmlSerializer for xsi:type attributes
        private static readonly XNamespace XsiNs =
            "http://www.w3.org/2001/XMLSchema-instance";

        // XSD namespace (also appears in XmlSerializer output)
        private static readonly XNamespace XsdNs =
            "http://www.w3.org/2001/XMLSchema";

        /// <summary>
        /// Recursively converts a JsonElement back into an XElement.
        /// Keys starting with "@" become XML attributes; "#text" becomes element value.
        /// </summary>
        private static XElement JsonElementToXElement(string name, JsonElement el)
        {
            var node = new XElement(name);

            switch (el.ValueKind)
            {
                case JsonValueKind.Object:
                    foreach (JsonProperty prop in el.EnumerateObject())
                    {
                        if (prop.Name.StartsWith("@"))
                        {
                            // Restore XML attribute
                            string attrLocalName = prop.Name[1..]; // strip leading "@"
                            if (attrLocalName == "type")
                            {
                                // xsi:type needs the xsi namespace
                                node.Add(new XAttribute(XsiNs + "type", prop.Value.GetString() ?? ""));
                            }
                            else if (attrLocalName == "nil")
                            {
                                node.Add(new XAttribute(XsiNs + "nil", prop.Value.GetString() ?? ""));
                            }
                            else
                            {
                                node.Add(new XAttribute(attrLocalName, prop.Value.GetString() ?? ""));
                            }
                        }
                        else if (prop.Name == "#text")
                        {
                            // Restore text content of element that also had attributes
                            node.Value = prop.Value.GetString() ?? "";
                        }
                        else
                        {
                            // Regular child element
                            node.Add(JsonElementToXElement(prop.Name, prop.Value));
                        }
                    }
                    break;

                case JsonValueKind.Array:
                    // Each array item becomes a child with the singular form of the parent name
                    string childName = DeriveSingular(name);
                    foreach (JsonElement item in el.EnumerateArray())
                        node.Add(JsonElementToXElement(childName, item));
                    break;

                default:
                    // Primitive: string, number, bool, null
                    node.Value = el.ToString() ?? "";
                    break;
            }

            return node;
        }

        /// <summary>
        /// Derives a singular element name from a plural/array wrapper name.
        /// "ArrayOfCosmeticProduct" → "CosmeticProduct"
        /// "Items" → "Item"
        /// </summary>
        private static string DeriveSingular(string name)
        {
            const string prefix = "ArrayOf";
            if (name.StartsWith(prefix, StringComparison.Ordinal))
                return name[prefix.Length..];
            if (name.EndsWith("s", StringComparison.Ordinal) && name.Length > 1)
                return name[..^1];
            return name;
        }
    }
}