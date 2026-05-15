using System;
using System.Collections.Generic;
using loriks3;

namespace loriks3.PluginContracts
{
    /// <summary>
    /// Contract that every plugin assembly must implement.
    /// The host application discovers and loads plugins via this interface
    /// without any compile-time dependency on the plugin code.
    /// </summary>
    public interface ICosmeticPlugin
    {
        /// <summary>Display name shown in the UI type combo-box.</summary>
        string TypeName { get; }

        /// <summary>
        /// Creates a new default instance of the product this plugin provides.
        /// The returned object must derive from CosmeticProduct.
        /// </summary>
        loriks3.CosmeticProduct CreateDefault();
    }
}