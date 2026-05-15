using System.Collections.Generic;
using System.Xml.Serialization;
using loriks3;
using loriks3.PluginContracts;

namespace Plugin.HairCare
{
    // ─────────────────────────────────────────────────────────────────────────
    //  Plugin: HairCare  –  adds the HairMask product type to the catalog.
    //  Drop Plugin.HairCare.dll into the <exe>/Plugins/ folder and restart.
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Concrete product: Hair Mask.
    /// Extends CosmeticProduct with hair type, treatment goal and leave-in flag.
    /// </summary>
    [XmlType("HairMask")]
    public class HairMask : CosmeticProduct
    {
        /// <summary>Target hair type: Dry, Oily, Curly, Color-treated, All, etc.</summary>
        public string HairType { get; set; } = "";

        /// <summary>Primary treatment benefit: Repair, Moisture, Shine, Volume, etc.</summary>
        public string TreatmentGoal { get; set; } = "";

        /// <summary>True if the product is a leave-in treatment; false = rinse-out.</summary>
        public bool IsLeaveIn { get; set; }

        // ── CosmeticProduct overrides ─────────────────────────────────────────

        /// <inheritdoc/>
        public override string GetDetails() =>
            $"Name={Name}|Brand={Brand}|Price={Price}|Color={Color}" +
            $"|HairType={HairType}|TreatmentGoal={TreatmentGoal}|IsLeaveIn={IsLeaveIn}";

        /// <inheritdoc/>
        public override List<string> GetEditableFields() =>
            new() { "Name", "Brand", "Price", "Color", "HairType", "TreatmentGoal", "IsLeaveIn" };

        /// <inheritdoc/>
        public override Dictionary<string, string> GetFieldValues() =>
            new()
            {
                ["Name"]          = Name,
                ["Brand"]         = Brand,
                ["Price"]         = Price.ToString(),
                ["Color"]         = Color,
                ["HairType"]      = HairType,
                ["TreatmentGoal"] = TreatmentGoal,
                ["IsLeaveIn"]     = IsLeaveIn.ToString(),
            };

        /// <inheritdoc/>
        public override void SetDetails(Dictionary<string, string> v)
        {
            Name          = v.GetValueOrDefault("Name",          Name);
            Brand         = v.GetValueOrDefault("Brand",         Brand);
            if (decimal.TryParse(v.GetValueOrDefault("Price", "0"), out var p)) Price = p;
            Color         = v.GetValueOrDefault("Color",         Color);
            HairType      = v.GetValueOrDefault("HairType",      HairType);
            TreatmentGoal = v.GetValueOrDefault("TreatmentGoal", TreatmentGoal);
            if (bool.TryParse(v.GetValueOrDefault("IsLeaveIn", "false"), out var li)) IsLeaveIn = li;
        }
    }

    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Plugin entry-point discovered by <c>PluginLoader</c>.
    /// Creates a default HairMask instance for the factory map.
    /// </summary>
    public sealed class HairCarePlugin : ICosmeticPlugin
    {
        /// <inheritdoc/>
        public string TypeName => "Hair Mask";

        /// <inheritdoc/>
        public CosmeticProduct CreateDefault() => new HairMask
        {
            Name          = "New Hair Mask",
            Brand         = "Brand",
            Price         = 0,
            Color         = "White",
            HairType      = "All",
            TreatmentGoal = "Repair",
            IsLeaveIn     = false,
        };
    }
}
