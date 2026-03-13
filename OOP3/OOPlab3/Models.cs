using System.Xml.Serialization;

namespace loriks3
{
    // Base abstract class for all cosmetic products
    [XmlInclude(typeof(Lipstick))]
    [XmlInclude(typeof(Foundation))]
    [XmlInclude(typeof(Mascara))]
    [XmlInclude(typeof(Eyeshadow))]
    [XmlInclude(typeof(Perfume))]
    [XmlInclude(typeof(Blush))]
    public abstract class CosmeticProduct
    {
        public string Name { get; set; } = "";
        public string Brand { get; set; } = "";
        public decimal Price { get; set; }
        public string Color { get; set; } = "";

        // Returns display string for ListBox
        public override string ToString() => $"[{GetType().Name}] {Brand} - {Name}";

        // Returns all properties as string for editing display
        public abstract string GetDetails();

        // Parses edited properties back from string values
        public abstract void SetDetails(Dictionary<string, string> values);

        // Returns list of editable property names
        public abstract List<string> GetEditableFields();

        // Returns current values of editable fields
        public abstract Dictionary<string, string> GetFieldValues();
    }

    // Lip cosmetics category
    public abstract class LipProduct : CosmeticProduct
    {
        public string Finish { get; set; } = ""; // matte, glossy, satin
    }

    // Eye cosmetics category
    public abstract class EyeProduct : CosmeticProduct
    {
        public bool IsWaterproof { get; set; }
    }

    // Concrete class: Lipstick
    public class Lipstick : LipProduct
    {
        public string Texture { get; set; } = ""; // creamy, velvet, liquid

        public override string GetDetails() =>
            $"Name={Name}|Brand={Brand}|Price={Price}|Color={Color}|Finish={Finish}|Texture={Texture}";

        public override List<string> GetEditableFields() =>
            new() { "Name", "Brand", "Price", "Color", "Finish", "Texture" };

        public override Dictionary<string, string> GetFieldValues() =>
            new() { ["Name"] = Name, ["Brand"] = Brand, ["Price"] = Price.ToString(), ["Color"] = Color, ["Finish"] = Finish, ["Texture"] = Texture };

        public override void SetDetails(Dictionary<string, string> v)
        {
            Name = v.GetValueOrDefault("Name", Name);
            Brand = v.GetValueOrDefault("Brand", Brand);
            if (decimal.TryParse(v.GetValueOrDefault("Price", "0"), out var p)) Price = p;
            Color = v.GetValueOrDefault("Color", Color);
            Finish = v.GetValueOrDefault("Finish", Finish);
            Texture = v.GetValueOrDefault("Texture", Texture);
        }
    }

    // Concrete class: Foundation
    public class Foundation : CosmeticProduct
    {
        public string Coverage { get; set; } = ""; // light, medium, full
        public string Shade { get; set; } = "";

        public override string GetDetails() =>
            $"Name={Name}|Brand={Brand}|Price={Price}|Color={Color}|Coverage={Coverage}|Shade={Shade}";

        public override List<string> GetEditableFields() =>
            new() { "Name", "Brand", "Price", "Color", "Coverage", "Shade" };

        public override Dictionary<string, string> GetFieldValues() =>
            new() { ["Name"] = Name, ["Brand"] = Brand, ["Price"] = Price.ToString(), ["Color"] = Color, ["Coverage"] = Coverage, ["Shade"] = Shade };

        public override void SetDetails(Dictionary<string, string> v)
        {
            Name = v.GetValueOrDefault("Name", Name);
            Brand = v.GetValueOrDefault("Brand", Brand);
            if (decimal.TryParse(v.GetValueOrDefault("Price", "0"), out var p)) Price = p;
            Color = v.GetValueOrDefault("Color", Color);
            Coverage = v.GetValueOrDefault("Coverage", Coverage);
            Shade = v.GetValueOrDefault("Shade", Shade);
        }
    }

    // Concrete class: Mascara
    public class Mascara : EyeProduct
    {
        public string Effect { get; set; } = ""; // volumizing, lengthening, curling

        public override string GetDetails() =>
            $"Name={Name}|Brand={Brand}|Price={Price}|Color={Color}|IsWaterproof={IsWaterproof}|Effect={Effect}";

        public override List<string> GetEditableFields() =>
            new() { "Name", "Brand", "Price", "Color", "IsWaterproof", "Effect" };

        public override Dictionary<string, string> GetFieldValues() =>
            new() { ["Name"] = Name, ["Brand"] = Brand, ["Price"] = Price.ToString(), ["Color"] = Color, ["IsWaterproof"] = IsWaterproof.ToString(), ["Effect"] = Effect };

        public override void SetDetails(Dictionary<string, string> v)
        {
            Name = v.GetValueOrDefault("Name", Name);
            Brand = v.GetValueOrDefault("Brand", Brand);
            if (decimal.TryParse(v.GetValueOrDefault("Price", "0"), out var p)) Price = p;
            Color = v.GetValueOrDefault("Color", Color);
            if (bool.TryParse(v.GetValueOrDefault("IsWaterproof", "false"), out var w)) IsWaterproof = w;
            Effect = v.GetValueOrDefault("Effect", Effect);
        }
    }

    // Concrete class: Eyeshadow palette
    public class Eyeshadow : EyeProduct
    {
        public int PanCount { get; set; } // number of shades in palette

        public override string GetDetails() =>
            $"Name={Name}|Brand={Brand}|Price={Price}|Color={Color}|IsWaterproof={IsWaterproof}|PanCount={PanCount}";

        public override List<string> GetEditableFields() =>
            new() { "Name", "Brand", "Price", "Color", "IsWaterproof", "PanCount" };

        public override Dictionary<string, string> GetFieldValues() =>
            new() { ["Name"] = Name, ["Brand"] = Brand, ["Price"] = Price.ToString(), ["Color"] = Color, ["IsWaterproof"] = IsWaterproof.ToString(), ["PanCount"] = PanCount.ToString() };

        public override void SetDetails(Dictionary<string, string> v)
        {
            Name = v.GetValueOrDefault("Name", Name);
            Brand = v.GetValueOrDefault("Brand", Brand);
            if (decimal.TryParse(v.GetValueOrDefault("Price", "0"), out var p)) Price = p;
            Color = v.GetValueOrDefault("Color", Color);
            if (bool.TryParse(v.GetValueOrDefault("IsWaterproof", "false"), out var w)) IsWaterproof = w;
            if (int.TryParse(v.GetValueOrDefault("PanCount", "1"), out var c)) PanCount = c;
        }
    }

    // Concrete class: Perfume
    public class Perfume : CosmeticProduct
    {
        public string Notes { get; set; } = ""; // top, mid, base notes
        public int VolumeML { get; set; }

        public override string GetDetails() =>
            $"Name={Name}|Brand={Brand}|Price={Price}|Color={Color}|Notes={Notes}|VolumeML={VolumeML}";

        public override List<string> GetEditableFields() =>
            new() { "Name", "Brand", "Price", "Color", "Notes", "VolumeML" };

        public override Dictionary<string, string> GetFieldValues() =>
            new() { ["Name"] = Name, ["Brand"] = Brand, ["Price"] = Price.ToString(), ["Color"] = Color, ["Notes"] = Notes, ["VolumeML"] = VolumeML.ToString() };

        public override void SetDetails(Dictionary<string, string> v)
        {
            Name = v.GetValueOrDefault("Name", Name);
            Brand = v.GetValueOrDefault("Brand", Brand);
            if (decimal.TryParse(v.GetValueOrDefault("Price", "0"), out var p)) Price = p;
            Color = v.GetValueOrDefault("Color", Color);
            Notes = v.GetValueOrDefault("Notes", Notes);
            if (int.TryParse(v.GetValueOrDefault("VolumeML", "50"), out var ml)) VolumeML = ml;
        }
    }

    // Concrete class: Blush
    public class Blush : LipProduct
    {
        public string Formula { get; set; } = ""; // powder, cream, liquid

        public override string GetDetails() =>
            $"Name={Name}|Brand={Brand}|Price={Price}|Color={Color}|Finish={Finish}|Formula={Formula}";

        public override List<string> GetEditableFields() =>
            new() { "Name", "Brand", "Price", "Color", "Finish", "Formula" };

        public override Dictionary<string, string> GetFieldValues() =>
            new() { ["Name"] = Name, ["Brand"] = Brand, ["Price"] = Price.ToString(), ["Color"] = Color, ["Finish"] = Finish, ["Formula"] = Formula };

        public override void SetDetails(Dictionary<string, string> v)
        {
            Name = v.GetValueOrDefault("Name", Name);
            Brand = v.GetValueOrDefault("Brand", Brand);
            if (decimal.TryParse(v.GetValueOrDefault("Price", "0"), out var p)) Price = p;
            Color = v.GetValueOrDefault("Color", Color);
            Finish = v.GetValueOrDefault("Finish", Finish);
            Formula = v.GetValueOrDefault("Formula", Formula);
        }
    }
}
