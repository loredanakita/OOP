using System.Collections.Generic;
using System.Xml.Serialization;
using loriks3;
using loriks3.PluginContracts;

namespace Plugin.Skincare
{
    // ─────────────────────────────────────────────────────────────────────────
    //  Plugin: Skincare  –  adds the FaceSerum product type to the catalog.
    //  Drop Plugin.Skincare.dll into the <exe>/Plugins/ folder and restart.
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Concrete product: Face Serum.
    /// Extends CosmeticProduct with skin concern, active ingredients, and
    /// SPF field (optional sun-protection factor, 0 = none).
    /// </summary>
    [XmlType("FaceSerum")]
    public class FaceSerum : CosmeticProduct
    {
        /// <summary>Target skin concern: Hydration, Anti-aging, Brightening, Acne, etc.</summary>
        public string SkinConcern { get; set; } = "";

        /// <summary>Key active ingredients, comma-separated (e.g. "Vitamin C, Niacinamide").</summary>
        public string ActiveIngredients { get; set; } = "";

        /// <summary>SPF rating (0 means no sun protection).</summary>
        public int SPF { get; set; }

        // ── CosmeticProduct overrides ─────────────────────────────────────────

        /// <inheritdoc/>
        public override string GetDetails() =>
            $"Name={Name}|Brand={Brand}|Price={Price}|Color={Color}" +
            $"|SkinConcern={SkinConcern}|ActiveIngredients={ActiveIngredients}|SPF={SPF}";

        /// <inheritdoc/>
        public override List<string> GetEditableFields() =>
            new() { "Name", "Brand", "Price", "Color", "SkinConcern", "ActiveIngredients", "SPF" };

        /// <inheritdoc/>
        public override Dictionary<string, string> GetFieldValues() =>
            new()
            {
                ["Name"]             = Name,
                ["Brand"]            = Brand,
                ["Price"]            = Price.ToString(),
                ["Color"]            = Color,
                ["SkinConcern"]      = SkinConcern,
                ["ActiveIngredients"]= ActiveIngredients,
                ["SPF"]              = SPF.ToString(),
            };

        /// <inheritdoc/>
        public override void SetDetails(Dictionary<string, string> v)
        {
            Name              = v.GetValueOrDefault("Name",              Name);
            Brand             = v.GetValueOrDefault("Brand",             Brand);
            if (decimal.TryParse(v.GetValueOrDefault("Price", "0"), out var p)) Price = p;
            Color             = v.GetValueOrDefault("Color",             Color);
            SkinConcern       = v.GetValueOrDefault("SkinConcern",       SkinConcern);
            ActiveIngredients = v.GetValueOrDefault("ActiveIngredients", ActiveIngredients);
            if (int.TryParse(v.GetValueOrDefault("SPF", "0"), out var spf)) SPF = spf;
        }
    }

    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Plugin entry-point discovered by <c>PluginLoader</c>.
    /// Creates a default FaceSerum instance for the factory map.
    /// </summary>
    public sealed class SkincarePlugin : ICosmeticPlugin
    {
        /// <inheritdoc/>
        public string TypeName => "Face Serum";

        /// <inheritdoc/>
        public CosmeticProduct CreateDefault() => new FaceSerum
        {
            Name              = "New Face Serum",
            Brand             = "Brand",
            Price             = 0,
            Color             = "Clear",
            SkinConcern       = "Hydration",
            ActiveIngredients = "Hyaluronic Acid",
            SPF               = 0,
        };
    }
}
