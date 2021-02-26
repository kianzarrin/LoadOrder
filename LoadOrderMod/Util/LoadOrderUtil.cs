using ColossalFramework;
using KianCommons;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using static ColossalFramework.Plugins.PluginManager;
using LoadOrder;

namespace LoadOrderMod.Util {
    internal static class LoadOrderUtil {
        public static LoadOrderConfig Config;

        internal static int GetLoadOrder(this PluginInfo p) {
            var mod = p.GetModConfig();
            if(mod == null)
                return LoadOrderConfig.DefaultLoadOrder;
            return mod.LoadOrder;
        }

        internal static LoadOrder.ModInfo GetModConfig(this PluginInfo p)=>
            Config.Mods?.FirstOrDefault(_mod => _mod.Path == p.modPath);

        internal static string DllName(this PluginInfo p) =>
            p.userModInstance?.GetType()?.Assembly?.GetName()?.Name;
        internal static bool IsHarmonyMod(this PluginInfo p) =>
            p.name == "2040656402" || p.name == "CitiesHarmony";
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
