using ColossalFramework.PlatformServices;
using ColossalFramework.Plugins;
using ICities;
using KianCommons;
using System;
using static KianCommons.ReflectionHelpers;

namespace LoadOrderMod.Settings {
    public static class Settings {
        public static void OnSettingsUI(UIHelper helper) {
            Log.Debug("PluginManager.m_EventsEnabled=" +
                GetFieldValue<PluginManager>("m_EventsEnabled"));
            Log.Debug(Environment.StackTrace);
            //GUI.Settings.OnSettingsUI(helper);
            helper.AddButton("RequestItemDetails", OnRequestItemDetailsClicked);
            //helper.AddButton("QueryItems", OnQueryItemsClicked);
            helper.AddButton("RunCallbacks", OnRunCallbacksClicked);
            helper.AddButton("Enable Bufferred Log", () => Log.Buffered = true);
            helper.AddButton("Disable Buffered Log", () => Log.Buffered = false);
            //PlatformService.workshop.eventUGCQueryCompleted -= OnUGCQueryCompleted;
            //PlatformService.workshop.eventUGCRequestUGCDetailsCompleted -= OnUGCRequestUGCDetailsCompleted;
            //PlatformService.workshop.eventUGCQueryCompleted += OnUGCQueryCompleted;
            //PlatformService.workshop.eventUGCRequestUGCDetailsCompleted += OnUGCRequestUGCDetailsCompleted;
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
