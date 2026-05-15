using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Xsl;

// ─────────────────────────────────────────────────────────────────────────────
//  "Friend's plugin" — Lab 5, Variant 4: XML Transformation via XSLT
//  This assembly simulates a plugin received from a classmate.
//  It exposes a custom interface (IXsltTransformer) that is NOT IStoragePlugin,
//  so an Adapter is required to use it in the host application.
// ─────────────────────────────────────────────────────────────────────────────

namespace Plugin.XsltTransform
{
    /// <summary>
    /// Friend's own interface — unknown to the host application.
    /// The host cannot use this directly without an Adapter.
    /// </summary>
    public interface IXsltTransformer
    {
        string PluginName { get; }
        string PluginDescription { get; }

        /// <summary>Applies an XSLT stylesheet to the given XML and returns transformed XML.</summary>
        string Transform(string xmlInput, string xsltContent);

        /// <summary>Reverses the transformation (identity — returns XML as-is since XSLT is one-way).</summary>
        string ReverseTransform(string transformedContent);

        /// <summary>Returns the default XSLT stylesheet used by this plugin.</summary>
        string GetDefaultXslt();

        /// <summary>Returns default configuration key-value pairs.</summary>
        Dictionary<string, string> GetDefaults();
    }

    /// <summary>
    /// Concrete XSLT transformer plugin (friend's implementation).
    /// Applies a configurable XSLT stylesheet to XML before saving.
    /// On load the file is already valid XML, so no reverse transform is needed.
    /// </summary>
    public sealed class XsltTransformPlugin : IXsltTransformer
    {
        // ── IXsltTransformer ──────────────────────────────────────────────────

        /// <inheritdoc/>
        public string PluginName => "XSLT Transform";

        /// <inheritdoc/>
        public string PluginDescription =>
            "Applies an XSLT stylesheet to XML data before saving. " +
            "Useful for restructuring or filtering the XML output.";

        /// <inheritdoc/>
        public string GetDefaultXslt() =>
            """
            <?xml version="1.0" encoding="utf-8"?>
            <xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

              <!-- Identity transform: copies every node unchanged.
                   Replace or extend this stylesheet to customise the output. -->
              <xsl:output method="xml" indent="yes" encoding="utf-8"/>

              <xsl:template match="@* | node()">
                <xsl:copy>
                  <xsl:apply-templates select="@* | node()"/>
                </xsl:copy>
              </xsl:template>

              <!-- Example: add a "transformed" attribute to every product element -->
              <xsl:template match="CosmeticProduct">
                <xsl:copy>
                  <xsl:attribute name="transformed">true</xsl:attribute>
                  <xsl:apply-templates select="@* | node()"/>
                </xsl:copy>
              </xsl:template>

            </xsl:stylesheet>
            """;

        /// <inheritdoc/>
        public Dictionary<string, string> GetDefaults() => new()
        {
            // Inline XSLT stylesheet — the user can replace it in Plugin Settings
            ["Xslt"] = GetDefaultXslt(),
        };

        /// <summary>
        /// Applies the XSLT from <paramref name="xsltContent"/> to <paramref name="xmlInput"/>.
        /// Returns the transformed XML string.
        /// </summary>
        public string Transform(string xmlInput, string xsltContent)
        {
            // Load the XSLT stylesheet
            var xslt = new XslCompiledTransform();
            using (var xsltReader = XmlReader.Create(new StringReader(xsltContent)))
                xslt.Load(xsltReader);

            // Apply the transform
            using var inputReader = XmlReader.Create(new StringReader(xmlInput));
            var sb = new StringBuilder();
            var settings = new XmlWriterSettings { Indent = true, Encoding = Encoding.UTF8 };
            using (var writer = XmlWriter.Create(sb, settings))
                xslt.Transform(inputReader, writer);

            return sb.ToString();
        }

        /// <summary>
        /// XSLT transforms are generally one-way; on load we return the XML unchanged
        /// because the output of our stylesheet is still valid XML for XmlSerializer.
        /// </summary>
        public string ReverseTransform(string transformedContent) => transformedContent;
    }
}
