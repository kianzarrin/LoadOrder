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
        public static int GetLoadOrder(this PluginInfo p) {
            var savedLoadIndexKey = p.name + p.modPath.GetHashCode() + ".Order";
            SavedInt SavedOrder = new SavedInt(savedLoadIndexKey, LoadOrderSettingsFile, DEFAULT_ORDER);
            return SavedOrder.value;
        }
    }
}
