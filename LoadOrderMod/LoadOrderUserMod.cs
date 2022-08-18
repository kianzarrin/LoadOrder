namespace LoadOrderMod {
    extern alias Injections;
    using SteamUtilities = Injections.LoadOrderInjections.SteamUtilities;
    using CitiesHarmony.API;
    using ICities;
    using KianCommons;
    using System;
    using UnityEngine.SceneManagement;
    using LoadOrderMod.Util;
    using LoadOrderMod.UI;
    using UnityEngine;
    using ColossalFramework;
    using LoadOrderMod.Data;
    using ColossalFramework.PlatformServices;
    using System.Linq;
    using LoadOrderMod.UI.EntryStatus;
    using LoadOrderMod.UI.EntryAction;
    using ColossalFramework.Plugins;

    public class LoadOrderUserMod : IUserMod {
        public static Version ModVersion => typeof(LoadOrderUserMod).Assembly.GetName().Version;
        public static string VersionString => ModVersion.ToString(2);
        public string Name => "Load Order Mod " + VersionString;
        public string Description => "use LoadOrderTool.exe to manage the order in which mods are loaded.";
        public static string HARMONY_ID = "CS.Kian.LoadOrder";

        //static LoadOrderMod() => Log.Debug("Static Ctor "   + Environment.StackTrace);
        //public LoadOrderMod() => Log.Debug("Instance Ctor " + Environment.StackTrace);

        static bool HasDuplicate() {
            var currentASM = typeof(LoadOrderUserMod).Assembly;
            foreach (var plugin in PluginManager.instance.GetPluginsInfo()) {
                foreach(var a in plugin.GetAssemblies()) {
                    if (a != currentASM && a.Name() == currentASM.Name()) return true;
                }
            }
            return false;
        }

        void CheckDuplicate() {
            if (HasDuplicate()) {
                string m = "There are multiple versions of Load Order Mod. Please exluclude all but one.";
                Log.DisplayError(m);
                throw new Exception(m);
            }
        }

        public void OnEnabled() {
            CheckDuplicate();
            try {
                Log.Called();
 

                Util.LoadOrderUtil.ApplyGameLoggingImprovements();
                Log.Info("Cloud.enabled=" + (PlatformService.cloud?.enabled).ToSTR(), true);

                var args = Environment.GetCommandLineArgs();
                Log.Info("command line args are: " + string.Join(" ", args));

                Log.ShowGap = true;
#if DEBUG
                Log.Buffered = true;
#else
                Log.Buffered = true;
#endif
                var items = PlatformService.workshop.GetSubscribedItems();
                Log.Info("Subscribed Items are: " + items.ToSTR());

                //Log.Debug("Testing StackTrace:\n" + new StackTrace(true).ToString(), copyToGameLog: true);
                //KianCommons.UI.TextureUtil.EmbededResources = false;
                //HelpersExtensions.VERBOSE = false;
                //foreach(var p in ColossalFramework.Plugins.PluginManager.instance.GetPluginsInfo()) {
                //    string savedKey = p.name + p.modPath.GetHashCode().ToString() + ".enabled";
                //    Log.Debug($"plugin info: savedKey={savedKey} cachedName={p.name} modPath={p.modPath}");
                //}
                CheckPatchLoader();

                HarmonyHelper.DoOnHarmonyReady(() => {
                    //HarmonyLib.Harmony.DEBUG = true;
                    HarmonyUtil.InstallHarmony(HARMONY_ID, null, null); // continue on error.
                });
                SceneManager.sceneLoaded += OnSceneLoaded;
                SceneManager.activeSceneChanged += OnActiveSceneChanged;

                LoadingManager.instance.m_introLoaded += LoadOrderUtil.TurnOffSteamPanels;
                LoadingManager.instance.m_introLoaded += CheckPatchLoader;

                LoadOrderUtil.TurnOffSteamPanels();

                bool introLoaded = ContentManagerUtil.IsIntroLoaded;
                if (introLoaded) {
                    CacheUtil.CacheData();
                } else {
                    bool resetIsEnabledForAssets = Environment.GetCommandLineArgs().Any(_arg => _arg == "-reset-assets");
                    if (resetIsEnabledForAssets) {
                        LoadOrderUtil.ResetIsEnabledForAssets();
                    }
                    LoadingManager.instance.m_introLoaded += CacheUtil.CacheData;
                }


                if(!Settings.ConfigUtil.Config.IgnoranceIsBliss)
                    CheckSubsUtil.RegisterEvents();
                Log.Flush();
            } catch (Exception ex) {
                Log.Exception(ex);
            }
        }

        public void OnDisabled() {
            try {
                foreach (var item in GameObject.FindObjectsOfType<EntryStatusPanel>()) {
                    GameObject.DestroyImmediate(item?.gameObject);
                }
                foreach (var item in GameObject.FindObjectsOfType<EntryActionPanel>()) {
                    GameObject.DestroyImmediate(item?.gameObject);
                }

                LoadingManager.instance.m_introLoaded -= CacheUtil.CacheData;
                LoadingManager.instance.m_introLoaded -= LoadOrderUtil.TurnOffSteamPanels;
                LoadingManager.instance.m_introLoaded -= CheckPatchLoader;
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

        public void CheckPatchLoader() {
            Log.Info("SteamUtilities.Initialized=" + SteamUtilities.Initialized);
            if(!SteamUtilities.Initialized && PatchLoaderStatus.Instance.IsAvailbleAndEnabled) {
                Log.DisplayWarning("Patch Loader Ineffective. Some LOM features might not work!\n\n" + PatchLoaderStatus.WindowsCriticalErrorSolutions);
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
