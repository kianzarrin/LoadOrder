using ColossalFramework;
using KianCommons;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using static ColossalFramework.Plugins.PluginManager;
using LoadOrderShared;
using CitiesHarmony.API;
using ColossalFramework.IO;
using ColossalFramework.PlatformServices;
using ColossalFramework.Plugins;
using ICities;
using UnityEngine.SceneManagement;
using ColossalFramework.UI;
using static KianCommons.ReflectionHelpers;
using KianCommons.Plugins;

namespace LoadOrderMod.Util {
    internal static class LoadOrderUtil {
        public static LoadOrderConfig config_;
        public static LoadOrderConfig Config => config_ ??=
            LoadOrderConfig.Deserialize(DataLocation.localApplicationData)
            ?? new LoadOrderConfig();

        public static void SaveConfig() =>
            config_?.Serialize(DataLocation.localApplicationData);

        public static void SavePathDetails() {
            Config.GamePath = DataLocation.applicationBase;
            var plugin = PluginManager.instance.GetPluginsInfo()
                 .FirstOrDefault(_p => _p.publishedFileID != PublishedFileId.invalid);
            if(plugin?.modPath is string path) {
                Config.WorkShopContentPath = Path.GetDirectoryName(path); // get parent directory.
            }
            SaveConfig();
        }

        public static void SaveModDetails() {
            foreach(var pluginInfo in PluginManager.instance.GetPluginsInfo()) {
                var modInfo = Config.Mods.FirstOrDefault(_mod => _mod.Path == pluginInfo.modPath);
                modInfo ??= new LoadOrderShared.ModInfo {
                    Path = pluginInfo.modPath,
                    LoadOrder = LoadOrderConfig.DefaultLoadOrder,
                };
                modInfo.Description = pluginInfo.GetUserModInstance().Description;
                modInfo.ModName = pluginInfo.GetModName();
            }
            SaveConfig();
        }


        internal static int GetLoadOrder(this PluginInfo p) {
            var mod = p.GetModConfig();
            if(mod == null)
                return LoadOrderConfig.DefaultLoadOrder;
            return mod.LoadOrder;
        }

        internal static LoadOrderShared.ModInfo GetModConfig(this PluginInfo p)=>
            Config?.Mods?.FirstOrDefault(_mod => _mod.Path == p.modPath);

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

        public static void TurnOffSteamPanels() {
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
