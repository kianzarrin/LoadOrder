namespace LoadOrderMod.Settings {
    extern alias Injections;
    using ColossalFramework.PlatformServices;
    using ColossalFramework.UI;
    using KianCommons;
    using LoadOrderShared;
    using System;
    using UnityEngine;
    using UnityEngine.UI;
    using SteamUtilities = Injections.LoadOrderInjections.SteamUtilities;

    public static class Settings {
        static LoadOrderConfig Config => ConfigUtil.Config;

        public static void OnSettingsUI(UIHelper helper) {
            try {
                Subscriptions(helper);
                StartupSettings(helper);
                LogLoadingTimes(helper);
                Debug(helper);
            }catch(Exception ex) {
                Log.Exception(ex);
            }
        }

        public static void Debug(UIHelper helper) {
            Log.Debug(Environment.StackTrace);
            var g = helper.AddGroup("Developer") as UIHelper;
            g.AddCheckbox(
                "Soft assembly dependancy",
                ConfigUtil.Config.SoftDLLDependancy,
                val => {
                    ConfigUtil.Config.SoftDLLDependancy = val;
                    ConfigUtil.SaveConfig();
                });
            g.AddButton("Ensure All", CheckSubsUtil.EnsureAll);
            //g.AddButton("RequestItemDetails", OnRequestItemDetailsClicked);
            //g.AddButton("QueryItems", OnQueryItemsClicked);
            g.AddButton("RunCallbacks", OnRunCallbacksClicked);
            g.AddButton("Enable Bufferred Log", () => Log.Buffered = true);
            g.AddButton("Disable Buffered Log", () => Log.Buffered = false);
            //PlatformService.workshop.eventUGCQueryCompleted -= OnUGCQueryCompleted;
            //PlatformService.workshop.eventUGCRequestUGCDetailsCompleted -= OnUGCRequestUGCDetailsCompleted;
            //PlatformService.workshop.eventUGCQueryCompleted += OnUGCQueryCompleted;
            //PlatformService.workshop.eventUGCRequestUGCDetailsCompleted += OnUGCRequestUGCDetailsCompleted;
        }

        public static void Subscriptions(UIHelper helper) {
            var g = helper.AddGroup("Subscriptions") as UIHelper;
            UIButton button;
            UICheckBox checkBox;
            //g.AddButton("Perform All", OnPerformAllClicked);

            button = g.AddButton("Log bad workshop items", RequestItemDetails) as UIButton;
            button.tooltip = "checks for missing/partially downloaded/outdated items";

            checkBox = g.AddCheckbox(
                "Delete unsubscibed items on startup",
                Config.DeleteUnsubscribedItemsOnLoad,
                val => {
                    ConfigUtil.Config.DeleteUnsubscribedItemsOnLoad = val;
                    ConfigUtil.SaveConfig();
                }) as UICheckBox  ;

            button = g.AddButton("Delete Now", SteamUtilities.DeleteUnsubbed) as UIButton;
            Pairup(checkBox, button);

            //b = g.AddButton("delete duplicates", OnPerformAllClicked) as UIButton;
            //b.tooltip = "when excluded mod is updated, and included duplicate of it is created"; 
        }

        public static void StartupSettings(UIHelper helper) {
            var g = helper.AddGroup("Startup Settings (requires restart)");
            g.AddButton("Reset load orders", OnResetLoadOrdersClicked);

            g.AddCheckbox(
                "remove ad panels",
                ConfigUtil.Config.TurnOffSteamPanels,
                val => {
                    ConfigUtil.Config.TurnOffSteamPanels = val;
                    ConfigUtil.SaveConfig();
                });
            g.AddCheckbox(
                "Improve content manager",
                ConfigUtil.Config.FastContentManager,
                val => {
                    ConfigUtil.Config.FastContentManager = val;
                    ConfigUtil.SaveConfig();
                });
            g.AddCheckbox(
                "Add harmony resolver",
                ConfigUtil.Config.AddHarmonyResolver,
                val => {
                    ConfigUtil.Config.AddHarmonyResolver = val;
                    ConfigUtil.SaveConfig();
                });
        }


        static UIComponent logAssetLoadingTimesToggle_;
        static void LogLoadingTimes(UIHelper helper) {
            var g = helper.AddGroup("Log Loading Times");

            g.AddCheckbox(
                "Log asset loading times",
                ConfigUtil.Config.LogAssetLoadingTimes,
                val => {
                    ConfigUtil.Config.LogAssetLoadingTimes = val;
                    ConfigUtil.SaveConfig();
                    logAssetLoadingTimesToggle_.isEnabled = val;
                });

            logAssetLoadingTimesToggle_ = g.AddCheckbox(
                "Per Mod",
                ConfigUtil.Config.LogPerModAssetLoadingTimes,
                val => {
                    ConfigUtil.Config.LogPerModAssetLoadingTimes = val;
                    ConfigUtil.SaveConfig();
                }) as UIComponent;
            Indent(logAssetLoadingTimesToggle_);
        }

        public static void Indent(UIComponent c, int n=1) {

            if(c.Find<UILabel>("Label") is UILabel label)
                label.padding = new RectOffset(22*n, 0, 0, 0);
            
            if(c.Find<UISprite>("Unchecked") is UISprite check)
                check.relativePosition += new Vector3(22*n, 0);
        }

        public static void Pairup(UICheckBox checkbox, UIButton button) {
            var container = checkbox.parent as UIPanel;
            var panel = container.AddUIComponent<UIPanel>();
            panel.width = container.width;
            panel.height = button.height;

            checkbox.AlignTo(panel, UIAlignAnchor.TopLeft);
            checkbox.relativePosition += new Vector3(0, 10);
            button.AlignTo(panel, UIAlignAnchor.TopRight);
            button.relativePosition -= new Vector3(button.width, 0);
        }

        static void OnPerformAllClicked() {
            Log.Debug("Perform all pressed");
            CheckSubsUtil.EnsureAll();
            SteamUtilities.DeleteUnsubbed();
        }

        static void OnResetLoadOrdersClicked() {
            Log.Debug("Reset Load Orders pressed");
            foreach(var mod in Config.Mods)
                mod.LoadOrder = LoadOrderConfig.DefaultLoadOrder;
            ConfigUtil.SaveConfig();

        }

        static void OnRunCallbacksClicked() {
            Log.Debug("RunCallbacks pressed");
            try {
                PlatformService.RunCallbacks();
            } catch(Exception ex) {
                Log.Exception(ex);
            }
        }

        static void RequestItemDetails() {
            Log.Debug("RequestItemDetails pressed");
            foreach(var item in PlatformService.workshop.GetSubscribedItems()) {
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
