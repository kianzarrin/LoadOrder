namespace LoadOrderMod.Patches.HotReload {
    using HarmonyLib;
    using ColossalFramework.Plugins;
    using System.IO;

    [HarmonyPatch(typeof(PluginManager), "LoadPluginAtPath")]
    public static class LoadPluginAtPathPatch {
        /// <summary>
        /// pluginInfo.name that is being added. the name of the containing directory.
        /// (different than IUserMod.Name).
        /// </summary>        
        public static string name;

        static void Prefix(string path) {
            name = Path.GetFileNameWithoutExtension(path);
        }

        static void Finalizer() => name = null;
    }
}
