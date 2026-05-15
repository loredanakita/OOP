using System.Collections.Generic;
using System.Xml.Serialization;
using loriks3;
using loriks3.PluginContracts;

namespace Plugin.Nail
{
    // ─────────────────────────────────────────────────────────────────────────
    //  Plugin: Nail  –  adds the NailPolish product type to the catalog.
    //  Drop Plugin.Nail.dll into the <exe>/Plugins/ folder and restart the app;
    //  "Nail Polish" will appear in the type combo-box automatically.
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Concrete product: Nail Polish.
    /// Extends CosmeticProduct with finish type, formula and dry-time fields.
    /// XmlType is required so the shared XmlSerializer can round-trip this type
    /// even though it is defined outside the main assembly.
    /// </summary>
    [XmlType("NailPolish")]
    public class NailPolish : CosmeticProduct
    {
        /// <summary>Surface finish: Glossy, Matte, Shimmer, Glitter, etc.</summary>
        public string Finish { get; set; } = "";

        /// <summary>Formula type: Regular, Gel, Shellac, Dip Powder, etc.</summary>
        public string Formula { get; set; } = "";

        /// <summary>Approximate drying time in minutes.</summary>
        public int DryTimeMinutes { get; set; }

        // ── CosmeticProduct overrides ─────────────────────────────────────────

        /// <inheritdoc/>
        public override string GetDetails() =>
            $"Name={Name}|Brand={Brand}|Price={Price}|Color={Color}" +
            $"|Finish={Finish}|Formula={Formula}|DryTimeMinutes={DryTimeMinutes}";

        /// <inheritdoc/>
        public override List<string> GetEditableFields() =>
            new() { "Name", "Brand", "Price", "Color", "Finish", "Formula", "DryTimeMinutes" };

        /// <inheritdoc/>
        public override Dictionary<string, string> GetFieldValues() =>
            new()
            {
                ["Name"]           = Name,
                ["Brand"]          = Brand,
                ["Price"]          = Price.ToString(),
                ["Color"]          = Color,
                ["Finish"]         = Finish,
                ["Formula"]        = Formula,
                ["DryTimeMinutes"] = DryTimeMinutes.ToString(),
            };

        /// <inheritdoc/>
        public override void SetDetails(Dictionary<string, string> v)
        {
            Name    = v.GetValueOrDefault("Name",    Name);
            Brand   = v.GetValueOrDefault("Brand",   Brand);
            if (decimal.TryParse(v.GetValueOrDefault("Price", "0"), out var p))  Price = p;
            Color   = v.GetValueOrDefault("Color",   Color);
            Finish  = v.GetValueOrDefault("Finish",  Finish);
            Formula = v.GetValueOrDefault("Formula", Formula);
            if (int.TryParse(v.GetValueOrDefault("DryTimeMinutes", "5"), out var t)) DryTimeMinutes = t;
        }
    }

    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Plugin entry-point discovered by <c>PluginLoader</c>.
    /// Creates a default NailPolish instance for the factory map.
    /// </summary>
    public sealed class NailPlugin : ICosmeticPlugin
    {
        /// <inheritdoc/>
        public string TypeName => "Nail Polish";

        /// <inheritdoc/>
        public CosmeticProduct CreateDefault() => new NailPolish
        {
            Name           = "New Nail Polish",
            Brand          = "Brand",
            Price          = 0,
            Color          = "Red",
            Finish         = "Glossy",
            Formula        = "Regular",
            DryTimeMinutes = 10,
        };
    }
}
