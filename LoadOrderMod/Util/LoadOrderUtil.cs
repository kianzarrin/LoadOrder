using ColossalFramework;
using static ColossalFramework.Plugins.PluginManager;
using KianCommons;
using ICities;

namespace LoadOrderMod.Util {
    public static class LoadOrderUtil {
        static LoadOrderUtil() {
            if (GameSettings.FindSettingsFileByName(LoadOrderSettingsFile) == null) {
                GameSettings.AddSettingsFile(new SettingsFile[] { new SettingsFile() { fileName = LoadOrderSettingsFile } });
            }
        }

        public const string LoadOrderSettingsFile = "LoadOrder";
        const int DEFAULT_ORDER = 1000; // unordered plugins come last.

        public static SavedInt SavedLoadOrder(this PluginInfo p) {
            var savedLoadIndexKey = p.name + p.modPath.GetHashCode() + ".Order";
            return new SavedInt(savedLoadIndexKey, LoadOrderSettingsFile, DEFAULT_ORDER, autoUpdate:true);
        }
        public static int GetLoadOrder(this PluginInfo p) => p.SavedLoadOrder().value;
        public static string DllName(this PluginInfo p) => p.userModInstance?.GetType()?.Assembly?.GetName()?.Name;
        public static bool IsHarmonyMod(this PluginInfo p) => p.name == "2040656402";
    }
}
