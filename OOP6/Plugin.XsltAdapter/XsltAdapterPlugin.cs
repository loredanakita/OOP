using System;
using System.Collections.Generic;
using loriks3.PluginContracts;
using Plugin.XsltTransform;

// ─────────────────────────────────────────────────────────────────────────────
//  PATTERN: Adapter
//  Problem : The host application works with IStoragePlugin.
//            The friend's plugin implements IXsltTransformer — a different interface.
//  Solution: XsltAdapterPlugin wraps IXsltTransformer and exposes IStoragePlugin,
//            so the host can use the friend's plugin without any changes to the host code.
//
//  Class diagram:
//    IStoragePlugin  <──  XsltAdapterPlugin  ──>  IXsltTransformer
//                              (Adapter)              (Adaptee)
// ─────────────────────────────────────────────────────────────────────────────

namespace Plugin.XsltAdapter
{
    /// <summary>
    /// Adapter: bridges the friend's <see cref="IXsltTransformer"/> interface
    /// to the host application's <see cref="IStoragePlugin"/> contract.
    /// The host loads this DLL from the Plugins folder and treats it as
    /// a regular storage plugin — it never knows about IXsltTransformer.
    /// </summary>
    public sealed class XsltAdapterPlugin : IStoragePlugin
    {
        // ── Adaptee ───────────────────────────────────────────────────────────

        /// <summary>
        /// The friend's plugin instance being adapted.
        /// Composition is used (not inheritance) — the standard Adapter approach.
        /// </summary>
        private readonly IXsltTransformer _adaptee = new XsltTransformPlugin();

        // ── IStoragePlugin ────────────────────────────────────────────────────

        /// <inheritdoc/>
        public string Name => "XSLT Transform (Adapter)";

        /// <inheritdoc/>
        public string Description =>
            "Adapter for a friend's XSLT plugin. Applies an XSLT stylesheet to XML " +
            "before saving. Configure the stylesheet in Plugin Settings.";

        /// <summary>
        /// Exposes the friend's default settings through the host's interface.
        /// The key "Xslt" contains the inline stylesheet the user can edit.
        /// </summary>
        public Dictionary<string, string> GetDefaultSettings() => _adaptee.GetDefaults();

        /// <summary>
        /// Called by the host before writing to disk.
        /// Delegates to <see cref="IXsltTransformer.Transform"/> using the XSLT
        /// stylesheet stored in settings["Xslt"].
        /// </summary>
        public string ProcessBeforeSave(string xmlContent, Dictionary<string, string> settings)
        {
            // Retrieve the XSLT from settings; fall back to the default stylesheet
            string xslt = settings.TryGetValue("Xslt", out var s) && !string.IsNullOrWhiteSpace(s)
                ? s
                : _adaptee.GetDefaultXslt();

            // Adapt the call: IStoragePlugin.ProcessBeforeSave → IXsltTransformer.Transform
            return _adaptee.Transform(xmlContent, xslt);
        }

        /// <summary>
        /// Called by the host after reading from disk.
        /// XSLT output is still valid XML, so the adaptee returns it unchanged.
        /// </summary>
        public string ProcessAfterLoad(string fileContent, Dictionary<string, string> settings)
        {
            // Adapt the call: IStoragePlugin.ProcessAfterLoad → IXsltTransformer.ReverseTransform
            return _adaptee.ReverseTransform(fileContent);
        }
    }
}
