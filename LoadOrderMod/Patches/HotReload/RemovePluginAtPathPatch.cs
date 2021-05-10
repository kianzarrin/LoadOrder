namespace LoadOrderMod.Patches.HotReload {
    using HarmonyLib;
    using ColossalFramework.Plugins;
    using System.Collections.Generic;
    using static ColossalFramework.Plugins.PluginManager;

    [HarmonyPatch(typeof(PluginManager), "RemovePluginAtPath")]
    public static class RemovePluginAtPathPatch {
        /// <summary>
        /// pluginInfo.name that is being removed. the name of the containing directory.
        /// (different than IUserMod.Name).
        /// </summary>        
        public static string name;

        static void Prefix(string path, Dictionary<string, PluginInfo> ___m_Plugins) {
            if (___m_Plugins.TryGetValue(path, out var p) && p.isEnabled) {
                name = p.name;
            }
        }

        static void Finalizer() => name = null;
    }
}
