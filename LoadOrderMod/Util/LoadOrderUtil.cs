using ColossalFramework;
using ColossalFramework.Plugins;
using System;
using System.Text;
using static ColossalFramework.Plugins.PluginManager;
using KianCommons;

namespace LoadOrderMod.Util {
    public static class LoadOrderUtil {
        static LoadOrderUtil() {
            if (GameSettings.FindSettingsFileByName(LoadOrderSettingsFile) == null) {
                GameSettings.AddSettingsFile(new SettingsFile[] { new SettingsFile() { fileName = LoadOrderSettingsFile } });
            }
        }

        public static string LoadOrderSettingsFile => "LoadOrder";

        public static int GetLoadOrder(this PluginInfo p) {
            var dllName = p.GetMainAssembly().GetName().Name;
            var savedLoadIndexKey = dllName + ".Order";
            SavedInt SavedOrder = new SavedInt(savedLoadIndexKey, LoadOrderSettingsFile, 0);
            return SavedOrder.value;
        }
    }
}
