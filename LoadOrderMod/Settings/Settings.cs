namespace LoadOrderMod.Settings {
    using ColossalFramework.PlatformServices;
    using ColossalFramework.Plugins;
    using ICities;
    using KianCommons;
    using System;
    using static KianCommons.ReflectionHelpers;
    using LoadOrderMod.Util;
    using LoadOrderShared;
    public static class Settings {
        static LoadOrderConfig Config => ConfigUtil.Config;

        public static void OnSettingsUI(UIHelper helper) {




            Debug(helper);


        }

        public static void Debug(UIHelper helper) {
            Log.Debug(Environment.StackTrace);
            var g = helper.AddGroup("Maintanance");
            g.AddButton("Ensure All", CheckSubsUtil.EnsureAll);
            g.AddButton("RequestItemDetails", OnRequestItemDetailsClicked);
            //g.AddButton("QueryItems", OnQueryItemsClicked);
            g.AddButton("RunCallbacks", OnRunCallbacksClicked);
            g.AddButton("Enable Bufferred Log", () => Log.Buffered = true);
            g.AddButton("Disable Buffered Log", () => Log.Buffered = false);
            //PlatformService.workshop.eventUGCQueryCompleted -= OnUGCQueryCompleted;
            //PlatformService.workshop.eventUGCRequestUGCDetailsCompleted -= OnUGCRequestUGCDetailsCompleted;
            //PlatformService.workshop.eventUGCQueryCompleted += OnUGCQueryCompleted;
            //PlatformService.workshop.eventUGCRequestUGCDetailsCompleted += OnUGCRequestUGCDetailsCompleted;
        }

        public static void LoadSettings(UIHelper helper) {
            var g = helper.AddGroup("Load Settings (requires restart)");

            g.AddCheckbox(
                "remove ad panels",
                ConfigUtil.Config.TurnOffSteamPanels,
                val => {
                    ConfigUtil.Config.TurnOffSteamPanels = val;
                    ConfigUtil.SaveConfig();
                });
            g.AddCheckbox(
                "Improve content manager",
                ConfigUtil.Config.ImproveContentManager,
                val => {
                    ConfigUtil.Config.ImproveContentManager = val;
                    ConfigUtil.SaveConfig();
                });
            g.AddCheckbox(
                "Soft assembly dependancy",
                ConfigUtil.Config.SoftDLLDependancy,
                val => {
                    ConfigUtil.Config.SoftDLLDependancy = val;
                    ConfigUtil.SaveConfig();
                });
            //g.AddCheckbox(
            //    "",
            //    ConfigUtil.Config.,
            //    val => {
            //        ConfigUtil.Config. = val;
            //        ConfigUtil.SaveConfig();
            //    });
        }

        public static void GameLoadSettings(UIHelper helper) {
            helper.AddCheckbox(
                "Soft assembly dependancy",
                ConfigUtil.Config.SoftDLLDependancy,
                val => {
                    ConfigUtil.Config.SoftDLLDependancy = val;
                    ConfigUtil.SaveConfig();
    });
        }

        static void OnRunCallbacksClicked() {
            Log.Debug("RunCallbacks pressed");
            try {
                PlatformService.RunCallbacks();
            } catch (Exception ex) {
                Log.Exception(ex);
            }
        }

        static void OnRequestItemDetailsClicked() {
            Log.Debug("RequestItemDetails pressed");
            foreach (var item in PlatformService.workshop.GetSubscribedItems()) {
                PlatformService.workshop.RequestItemDetails(item).LogRet($"RequestItemDetails({item})");
            }
        }
        static void OnQueryItemsClicked() {
            Log.Debug("QueryItems pressed");
            PlatformService.workshop.QueryItems().LogRet($"QueryItems()"); ;
        }

        static void OnUGCQueryCompleted(UGCDetails result, bool ioError) {
            Log.Debug($"OnUGCQueryCompleted(result:{result.result} {result.publishedFileId}, ioError:{ioError})");
        }
        static void OnUGCRequestUGCDetailsCompleted(UGCDetails result, bool ioError) {
            Log.Debug($"OnUGCRequestUGCDetailsCompleted(result:{result.result} {result.publishedFileId}, ioError:{ioError})");
        }
    }
}
