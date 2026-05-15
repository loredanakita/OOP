using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using loriks3.PluginContracts;

namespace loriks3
{
    /// <summary>
    /// Discovers and loads plugin assemblies from the "Plugins" sub-folder
    /// located next to the main executable.
    /// Handles two plugin contracts:
    ///   • ICosmeticPlugin  — adds new product types to the catalog.
    ///   • IStoragePlugin   — transforms data before save / after load.
    /// </summary>
    public static class PluginLoader
    {
        // ── Public API ────────────────────────────────────────────────────────

        /// <summary>
        /// Scans the Plugins folder and returns every ICosmeticPlugin found.
        /// </summary>
        public static IReadOnlyList<ICosmeticPlugin> LoadAll() =>
            LoadAll<ICosmeticPlugin>();

        /// <summary>
        /// Scans the Plugins folder and returns every IStoragePlugin found.
        /// </summary>
        public static IReadOnlyList<IStoragePlugin> LoadStoragePlugins() =>
            LoadAll<IStoragePlugin>();

        // ── Generic scanner ───────────────────────────────────────────────────

        /// <summary>
        /// Generic implementation: scans all Plugin.*.dll files and collects
        /// every public concrete class that implements <typeparamref name="T"/>.
        /// Errors for individual assemblies are swallowed to keep the host stable.
        /// </summary>
        private static IReadOnlyList<T> LoadAll<T>()
        {
            var result = new List<T>();
            string pluginDir = GetPluginDirectory();
            if (!Directory.Exists(pluginDir)) return result;

            foreach (string dll in Directory.EnumerateFiles(pluginDir, "Plugin.*.dll"))
            {
                try
                {
                    result.AddRange(LoadFromAssembly<T>(dll));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(
                        $"[PluginLoader] Failed to load '{dll}': {ex.Message}");
                }
            }
            return result;
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        /// <summary>Returns the absolute path of the Plugins folder.</summary>
        private static string GetPluginDirectory() =>
            Path.Combine(AppContext.BaseDirectory, "Plugins");

        /// <summary>
        /// Reflects over a single DLL and instantiates every public concrete class
        /// that implements <typeparamref name="T"/> and has a parameterless constructor.
        /// </summary>
        private static IEnumerable<T> LoadFromAssembly<T>(string dllPath)
        {
            Assembly asm = Assembly.LoadFrom(dllPath);
            Type contractType = typeof(T);

            foreach (Type t in asm.GetExportedTypes())
            {
                if (t.IsAbstract || t.IsInterface) continue;
                if (!contractType.IsAssignableFrom(t)) continue;
                if (t.GetConstructor(Type.EmptyTypes) == null) continue;

                yield return (T)Activator.CreateInstance(t)!;
            }
        }
    }
}
