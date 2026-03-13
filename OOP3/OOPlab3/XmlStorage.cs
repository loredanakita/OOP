using System.Xml.Serialization;

namespace loriks3
{
    // Wrapper for XML serialization of polymorphic list
    [XmlRoot("CosmeticProducts")]
    public class ProductList
    {
        [XmlElement("Product")]
        public List<CosmeticProduct> Items { get; set; } = new();
    }

    // Handles XML save/load without if-else or reflection
    public static class XmlStorage
    {
        private static readonly XmlSerializer _serializer = new(typeof(ProductList));

        // Save list to XML file
        public static void Save(List<CosmeticProduct> items, string path)
        {
            var wrapper = new ProductList { Items = items };
            using var writer = new StreamWriter(path);
            _serializer.Serialize(writer, wrapper);
        }

        // Load list from XML file
        public static List<CosmeticProduct> Load(string path)
        {
            using var reader = new StreamReader(path);
            var wrapper = (ProductList)_serializer.Deserialize(reader)!;
            return wrapper.Items;
        }
    }
}
