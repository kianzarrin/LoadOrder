using ColossalFramework.IO;
using ColossalFramework.PlatformServices;
using ColossalFramework.Plugins;
using KianCommons;
using KianCommons.Plugins;
using LoadOrderShared;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using static ColossalFramework.Plugins.PluginManager;
using static KianCommons.ReflectionHelpers;
using HarmonyLib;

namespace LoadOrderMod.Util {
    internal static class LoadOrderUtil {
        public static LoadOrderConfig config_;
        public static LoadOrderConfig Config {
            get {
                try {
                    return config_ ??=
                        LoadOrderConfig.Deserialize(DataLocation.localApplicationData)
                        ?? new LoadOrderConfig();
                } catch(Exception ex) {
                    Log.Exception(ex);
                    return null;
                }
            }
        }

        public static void SaveConfig() =>
            config_?.Serialize(DataLocation.localApplicationData);

        public static void GetPathDetails() {
            try {
                Config.GamePath = DataLocation.applicationBase;
                foreach(var pluginInfo in PluginManager.instance.GetPluginsInfo()) {
                    if(pluginInfo.publishedFileID != PublishedFileId.invalid) {
                        Config.WorkShopContentPath = Path.GetDirectoryName(pluginInfo.modPath);
                        break;
                    }
                }
                SaveConfig();
            } catch(Exception ex) {
                Log.Exception(ex);
            }
        }

        public static void GetModsDetails() {
            foreach(var pluginInfo in PluginManager.instance.GetPluginsInfo()) {
                try {
                    var modInfo = pluginInfo.GetModConfig();
                    if(modInfo == null) {
                        modInfo = new LoadOrderShared.ModInfo {
                            Path = pluginInfo.modPath,
                            LoadOrder = LoadOrderConfig.DefaultLoadOrder,
                        };
                        Config.Mods = Config.Mods.AddToArray(modInfo);
                    }
                    modInfo.Description = pluginInfo.GetUserModInstance().Description;
                    modInfo.ModName = pluginInfo.GetModName();
                } catch(Exception ex) {
                    Log.Exception(ex);
                }
            }
        }

        public static void StoreConifgDetails() {
            try {
                GetPathDetails();
                GetModsDetails();
                SaveConfig();
            } catch(Exception ex) {
                Log.Exception(ex);
            }
        }

        internal static bool HasLoadOrder(this PluginInfo p) {
            var mod = p.GetModConfig();
            if(mod == null)
                return false;
            return mod.LoadOrder != LoadOrderConfig.DefaultLoadOrder;
        }

        internal static int GetLoadOrder(this PluginInfo p) {
            var mod = p.GetModConfig();
            if(mod == null)
                return LoadOrderConfig.DefaultLoadOrder;
            return mod.LoadOrder;
        }

        internal static LoadOrderShared.ModInfo GetModConfig(this PluginInfo p) =>
            Config?.Mods?.FirstOrDefault(_mod => _mod.Path == p.modPath);

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
