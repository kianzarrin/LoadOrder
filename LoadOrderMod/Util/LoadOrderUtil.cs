using ColossalFramework;
using static ColossalFramework.Plugins.PluginManager;
using KianCommons;
using ICities;
using System.IO;

namespace LoadOrderMod.Util {
    public static class LoadOrderUtil {
        static LoadOrderUtil() {
            if (GameSettings.FindSettingsFileByName(LoadOrderSettingsFile) == null) {
                GameSettings.AddSettingsFile(new SettingsFile[] { new SettingsFile() { fileName = LoadOrderSettingsFile } });
                Log.Info("Added Settings file: "+ LoadOrderSettingsFile);
            }
        }

        public const string LoadOrderSettingsFile = "LoadOrder";
        const int DEFAULT_ORDER = 1000; // unordered plugins come last.

        public static SavedInt SavedLoadOrder(this PluginInfo p) {
            string parentDirName = Directory.GetParent(p.modPath).Name;
            var savedLoadIndexKey = p.name + "." + parentDirName + ".Order";
            var ret = new SavedInt(savedLoadIndexKey, LoadOrderSettingsFile, DEFAULT_ORDER, autoUpdate:true);
            _ = ret.value; //force sync
            return ret;
        }
        public static int GetLoadOrder(this PluginInfo p) => p.SavedLoadOrder().value;
        public static string DllName(this PluginInfo p) => p.userModInstance?.GetType()?.Assembly?.GetName()?.Name;
        public static bool IsHarmonyMod(this PluginInfo p) => p.name == "2040656402" || p.name =="CitiesHarmony";
        public static bool IsLSM(this PluginInfo p) => p.name == "667342976" || p.name == "LoadingScreenMod";
    }
}
