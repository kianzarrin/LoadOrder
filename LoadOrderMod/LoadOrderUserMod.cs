namespace LoadOrderMod {
    using CitiesHarmony.API;
    using ICities;
    using KianCommons;
    using System;
    using UnityEngine.SceneManagement;
    using LoadOrderMod.Util;
    using LoadOrderMod.UI;
    using UnityEngine;
    using ColossalFramework;

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

                var args = Environment.GetCommandLineArgs();
                Log.Info("comamnd line args are: " + string.Join(" ", args));

                Log.ShowGap = true;
#if DEBUG
                Log.Buffered = true; 
#else
                Log.Buffered = true;
#endif

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
                if (SceneManager.GetActiveScene().name == "IntroScreen")
                    Settings.ConfigUtil.StoreConfigDetails();

                CheckSubsUtil.RegisterEvents();
                Log.Flush();
            } catch (Exception ex) {
                Log.Exception(ex);
            }
        }

        public void OnDisabled() {
            try {
                foreach(var item in GameObject.FindObjectsOfType<EntryStatusPanel>()) {
                    GameObject.DestroyImmediate(item?.gameObject);
                }

                LoadingManager.instance.m_introLoaded -= LoadOrderUtil.TurnOffSteamPanels;
                LoadingManager.instance.m_introLoaded -= Settings.ConfigUtil.StoreConfigDetails;
                HarmonyUtil.UninstallHarmony(HARMONY_ID);
                MonoStatus.Release();
                LOMAssetDataExtension.Release();

                Settings.ConfigUtil.Terminate();
                CheckSubsUtil.RemoveEvents();
                Log.Buffered = false;
            } catch (Exception ex) {
                Log.Exception(ex);
            }
        }

        public static void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            Log.Info($"OnSceneLoaded({scene.name}, {mode})", true);
            if(scene.name == "MainMenu") {
                MonoStatus.Ensure();
            }
            Log.Flush();
        }

        public static void OnActiveSceneChanged(Scene from, Scene to) {
            Log.Info($"OnActiveSceneChanged({from.name}, {to.name})", true);
            Log.Flush();
            if (Helpers.InStartupMenu)
                LoadOrderUtil.TurnOffSteamPanels();
        }

        public void OnSettingsUI(UIHelperBase helper) => Settings.Settings.OnSettingsUI(helper as UIHelper);
    }
}
