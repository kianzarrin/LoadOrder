namespace LoadOrderMod
{
    using System;
    using ICities;
    using KianCommons;
    using System.Diagnostics;
    using ColossalFramework.IO;
    using ColossalFramework.Plugins;
    using ColossalFramework.PlatformServices;
    using ColossalFramework.UI;
    using System.Linq;
    using System.IO;
    using CitiesHarmony.API;
    using UnityEngine.SceneManagement;
    using Util;

    public class LoadOrderMod : IUserMod {
        public static Version ModVersion => typeof(LoadOrderMod).Assembly.GetName().Version;
        public static string VersionString => ModVersion.ToString(2);
        public string Name => "Load Order Mod " + VersionString;
        public string Description => "use LoadOrderTool.exe to manage the order in which mods are loaded.";
        public static string HARMONY_ID = "CS.Kian.LoadOrder";

        //static LoadOrderMod() => Log.Debug("Static Ctor "   + Environment.StackTrace);
        //public LoadOrderMod() => Log.Debug("Instance Ctor " + Environment.StackTrace);

        public void OnEnabled() {
            Log.ShowGap = true;
            Log.Buffered = true;
            Log.Debug("Testing StackTrace:\n" + new StackTrace(true).ToString(), copyToGameLog: false);
            //KianCommons.UI.TextureUtil.EmbededResources = false;
            //HelpersExtensions.VERBOSE = false;
            //foreach(var p in ColossalFramework.Plugins.PluginManager.instance.GetPluginsInfo()) {
            //    string savedKey = p.name + p.modPath.GetHashCode().ToString() + ".enabled";
            //    Log.Debug($"plugin info: savedKey={savedKey} cachedName={p.name} modPath={p.modPath}");
            //}
            LoadOrderCache data = new LoadOrderCache { GamePath = DataLocation.applicationBase };

            var plugin = PluginManager.instance.GetPluginsInfo()
                 .FirstOrDefault(_p => _p.publishedFileID != PublishedFileId.invalid);
            if (plugin?.modPath is string path) {
                data.WorkShopContentPath = Path.GetDirectoryName(path); // get parent directory.
            }

            data.Serialize(DataLocation.localApplicationData);
            HarmonyHelper.DoOnHarmonyReady(() => HarmonyUtil.InstallHarmony(HARMONY_ID));
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.activeSceneChanged += OnActiveSceneChanged;

            Log.Flush();
            PlatformService.workshop.eventUGCQueryCompleted += OnUGCQueryCompleted;
            PlatformService.workshop.eventUGCRequestUGCDetailsCompleted += OnUGCRequestUGCDetailsCompleted;
        }

        public void OnDisabled() {
            Log.Buffered = false;
            HarmonyUtil.UninstallHarmony(HARMONY_ID);
            PlatformService.workshop.eventUGCQueryCompleted -= OnUGCQueryCompleted;
            PlatformService.workshop.eventUGCRequestUGCDetailsCompleted -= OnUGCRequestUGCDetailsCompleted;
        }

        public static void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            Log.Info($"SceneManager.sceneLoaded({scene.name}, {mode})", true);
            Log.Flush();
        }
        public static void OnActiveSceneChanged(Scene from, Scene to) {
            Log.Info($"SceneManager.activeSceneChanged({from.name}, {to.name})", true);
            Log.Flush();
        }


        public void OnSettingsUI(UIHelperBase helper) {
            //GUI.Settings.OnSettingsUI(helper);
            helper.AddButton("RequestItemDetails", OnRequestItemDetailsClicked);
            helper.AddButton("QueryItems", OnQueryItemsClicked);
        }

        static void OnRequestItemDetailsClicked() {
            Log.Debug("RequestItemDetails pressed");
            foreach(var item in PlatformService.workshop.GetSubscribedItems()) {
                PlatformService.workshop.RequestItemDetails(item);
            }
        }
        static void OnQueryItemsClicked() {
            Log.Debug("QueryItems pressed");
            PlatformService.workshop.QueryItems();
        }


        static void OnUGCQueryCompleted(UGCDetails result, bool ioError) {
            Log.Debug($"OnUGCQueryCompleted(result:{result.result} {result.publishedFileId}, ioError:{ioError})");
        }
        static void OnUGCRequestUGCDetailsCompleted(UGCDetails result, bool ioError) {
            Log.Debug($"OnUGCRequestUGCDetailsCompleted(result:{result.result} {result.publishedFileId}, ioError:{ioError})");
        }

    }
}
