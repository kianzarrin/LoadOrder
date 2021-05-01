namespace LoadOrderMod {
    using CitiesHarmony.API;
    using ColossalFramework.IO;
    using ColossalFramework.PlatformServices;
    using ColossalFramework.Plugins;
    using ICities;
    using KianCommons;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using UnityEngine.SceneManagement;
    using UnityEngine;
    using ColossalFramework.UI;
    using static KianCommons.ReflectionHelpers;
    using LoadOrderShared;
    using LoadOrderMod.Util;

    public class LoadOrderUserMod : IUserMod {
        public static Version ModVersion => typeof(LoadOrderUserMod).Assembly.GetName().Version;
        public static string VersionString => ModVersion.ToString(2);
        public string Name => "Load Order Mod " + VersionString;
        public string Description => "use LoadOrderTool.exe to manage the order in which mods are loaded.";
        public static string HARMONY_ID = "CS.Kian.LoadOrder";

        //static LoadOrderMod() => Log.Debug("Static Ctor "   + Environment.StackTrace);
        //public LoadOrderMod() => Log.Debug("Instance Ctor " + Environment.StackTrace);

        public void OnEnabled() {
            try {
                Util.LoadOrderUtil.ApplyGameLoggingImprovements();
                Log.ShowGap = true;
                Log.Buffered = true;
                //Log.Debug("Testing StackTrace:\n" + new StackTrace(true).ToString(), copyToGameLog: true);
                //KianCommons.UI.TextureUtil.EmbededResources = false;
                //HelpersExtensions.VERBOSE = false;
                //foreach(var p in ColossalFramework.Plugins.PluginManager.instance.GetPluginsInfo()) {
                //    string savedKey = p.name + p.modPath.GetHashCode().ToString() + ".enabled";
                //    Log.Debug($"plugin info: savedKey={savedKey} cachedName={p.name} modPath={p.modPath}");
                //}


                HarmonyHelper.DoOnHarmonyReady(() => {
                    //HarmonyLib.Harmony.DEBUG = true;
                    HarmonyUtil.InstallHarmony(HARMONY_ID, null, null); // continue on error.
                });
                SceneManager.sceneLoaded += OnSceneLoaded;
                SceneManager.activeSceneChanged += OnActiveSceneChanged;

                LoadingManager.instance.m_introLoaded += LoadOrderUtil.TurnOffSteamPanels;
                LoadOrderUtil.TurnOffSteamPanels();

                LoadingManager.instance.m_introLoaded += Settings.ConfigUtil.StoreConfigDetails;
                if(SceneManager.GetActiveScene().name == "IntroScreen")
                    Settings.ConfigUtil.StoreConfigDetails();

                Log.Flush();
            } catch(Exception ex) {
                Log.Exception(ex);
            }
        }


        public void OnDisabled() {
            try {
                LoadingManager.instance.m_introLoaded -= LoadOrderUtil.TurnOffSteamPanels;
                LoadingManager.instance.m_introLoaded -= Settings.ConfigUtil.StoreConfigDetails;
                HarmonyUtil.UninstallHarmony(HARMONY_ID);
                
                Settings.ConfigUtil.SaveThread.Terminate();
                Settings.ConfigUtil.config_ = null;
                Log.FlushTread.Terminate();
                Log.Buffered = false;
            } catch (Exception ex) {
                Log.Exception(ex);
            }
        }

        public static void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            Log.Info($"SceneManager.sceneLoaded({scene.name}, {mode})", true);
            Log.Flush();
        }
        public static void OnActiveSceneChanged(Scene from, Scene to) {
            Log.Info($"SceneManager.activeSceneChanged({from.name}, {to.name})", true);
            Log.Flush();
        }

        public void OnSettingsUI(UIHelperBase helper) => Settings.Settings.OnSettingsUI(helper as UIHelper);
    }
}
