using ColossalFramework;
using KianCommons;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using static ColossalFramework.Plugins.PluginManager;

namespace LoadOrderMod.Util {
    internal static class LoadOrderUtil {
        static LoadOrderUtil() {
            if (GameSettings.FindSettingsFileByName(LoadOrderSettingsFile) == null) {
                GameSettings.AddSettingsFile(new SettingsFile[] { new SettingsFile() { fileName = LoadOrderSettingsFile } });
                Log.Info("Added Settings file: " + LoadOrderSettingsFile);
            }
        }

        internal const string LoadOrderSettingsFile = "LoadOrder";
        const int DEFAULT_ORDER = 1000; // unordered plugins come last.

        public static SavedInt SavedLoadOrder(this PluginInfo p) {
            string parentDirName = Directory.GetParent(p.modPath).Name;
            var savedLoadIndexKey = p.name + "." + parentDirName + ".Order";
            var ret = new SavedInt(savedLoadIndexKey, LoadOrderSettingsFile, DEFAULT_ORDER, autoUpdate: true);
            _ = ret.value; //force sync
            return ret;
        }
        internal static int GetLoadOrder(this PluginInfo p) => p.SavedLoadOrder().value;
        internal static string DllName(this PluginInfo p) => p.userModInstance?.GetType()?.Assembly?.GetName()?.Name;
        internal static bool IsHarmonyMod(this PluginInfo p) => p.name == "2040656402" || p.name == "CitiesHarmony";
        internal static bool IsLSM(this PluginInfo p) => p.name == "667342976" || p.name == "LoadingScreenMod";

        internal static Assembly GetLSMAssembly() =>
            AppDomain.CurrentDomain.GetAssemblies()
            .FirstOrDefault(_asm => _asm.GetName().Name == "LoadingScreenMod");

        
        public static void ApplyGameLoggingImprovements()
        {
            Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
            Application.SetStackTraceLogType(LogType.Warning, StackTraceLogType.None);
            Application.SetStackTraceLogType(LogType.Error, StackTraceLogType.ScriptOnly);
            Application.SetStackTraceLogType(LogType.Exception, StackTraceLogType.ScriptOnly);
            Debug.Log("************************** Removed logging stacktrace bloat **************************");
            Debug.Log(Environment.StackTrace);
        }
    }
}
