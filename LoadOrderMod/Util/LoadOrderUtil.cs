using KianCommons;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using static ColossalFramework.Plugins.PluginManager;
using static KianCommons.ReflectionHelpers;

namespace LoadOrderMod.Util {
    internal static class LoadOrderUtil {
        static LoadOrderShared.LoadOrderConfig Config =>
            Settings.ConfigUtil.Config;
        internal static string DllName(this PluginInfo p) =>
            p.userModInstance?.GetType()?.Assembly?.GetName()?.Name;
        internal static bool IsHarmonyMod(this PluginInfo p) =>
            p.name == "2040656402" || p.name == "CitiesHarmony";
        internal static bool IsLSM(this PluginInfo p) => p.name == "667342976" || p.name == "LoadingScreenMod";

        internal static Assembly GetLSMAssembly() =>
            AppDomain.CurrentDomain.GetAssemblies()
            .FirstOrDefault(_asm => _asm.GetName().Name == "LoadingScreenMod");


        public static void ApplyGameLoggingImprovements() {
            Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
            Application.SetStackTraceLogType(LogType.Warning, StackTraceLogType.None);
            Application.SetStackTraceLogType(LogType.Error, StackTraceLogType.ScriptOnly);
            Application.SetStackTraceLogType(LogType.Exception, StackTraceLogType.ScriptOnly);
            Debug.Log("************************** Removed logging stacktrace bloat **************************");
            Debug.Log(Environment.StackTrace);
        }

        public static void TurnOffSteamPanels() {
            if(!Config.TurnOffSteamPanels) return;
            SetFieldValue<WorkshopAdPanel>("dontInitialize", true);
            Log.Info("Turning off steam panels", true);
            var news = GameObject.FindObjectOfType<NewsFeedPanel>();
            var ad = GameObject.FindObjectOfType<WorkshopAdPanel>();
            var dlc = GameObject.FindObjectOfType<DLCPanelNew>();
            var paradox = GameObject.FindObjectOfType<ParadoxAccountPanel>();
            GameObject.Destroy(news?.gameObject);
            GameObject.Destroy(ad?.gameObject);
            GameObject.Destroy(dlc?.gameObject);
            GameObject.Destroy(paradox?.gameObject);
        }

    }
}
