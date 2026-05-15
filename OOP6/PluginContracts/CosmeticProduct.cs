using System.Collections.Generic;
using System.Xml.Serialization;

namespace loriks3
{

    public abstract class CosmeticProduct
    {
        public string Name { get; set; } = "";
        public string Brand { get; set; } = "";
        public decimal Price { get; set; }
        public string Color { get; set; } = "";

        public override string ToString() => $"[{GetType().Name}] {Brand} - {Name}";
        public abstract string GetDetails();
        public abstract void SetDetails(Dictionary<string, string> values);
        public abstract List<string> GetEditableFields();
        public abstract Dictionary<string, string> GetFieldValues();
    }

    public abstract class LipProduct : CosmeticProduct
    {
        public string Finish { get; set; } = "";
    }

    public abstract class EyeProduct : CosmeticProduct
    {
        public bool IsWaterproof { get; set; }
    }
}
