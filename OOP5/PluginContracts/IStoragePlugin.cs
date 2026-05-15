using System;
using System.Collections.Generic;  

namespace loriks3.PluginContracts
{
    public interface IStoragePlugin
    {
        string Name { get; }
        string Description { get; }

        string ProcessBeforeSave(string xmlContent, Dictionary<string, string> settings);
        string ProcessAfterLoad(string fileContent, Dictionary<string, string> settings);
        Dictionary<string, string> GetDefaultSettings();
    }
}
