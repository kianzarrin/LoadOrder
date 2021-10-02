namespace LoadOrderMod.Settings.Tabs {
    extern alias Injections;
    using ColossalFramework.PlatformServices;
    using ColossalFramework.UI;
    using KianCommons;
    using LoadOrderShared;
    using LoadOrderMod.Util;
    using SteamUtilities = Injections.LoadOrderInjections.SteamUtilities;

    static class SubscriptionsTab {
        static LoadOrderConfig Config => ConfigUtil.Config;

        public static void Make(ExtUITabstrip tabStrip) {
            UIHelper panelHelper = tabStrip.AddTabPage("Subscriptions");

            UIButton button;
            UICheckBox checkBox;
            //g.AddButton("Perform All", OnPerformAllClicked);

            button = panelHelper.AddButton("Refresh workshop items (checks for bad items)", RequestItemDetails) as UIButton;
            button.tooltip = "checks for missing/partially downloaded/outdated items";

            button = panelHelper.AddButton("[experimental] unsubscribe from depricated workshop items", () => CheckSubsUtil.Instance.UnsubDepricated()) as UIButton;
            button.tooltip = "if steam does not return item path, i assume its depricated.";

            button = panelHelper.AddButton("Resubribe to all broken downloads (exits game)", CheckSubsUtil.ResubcribeExternally) as UIButton;
            button.tooltip = "less steam can hide problems. if you use less steam please click 'Refresh workshop items' to get all broken downloads";

            checkBox = panelHelper.AddCheckbox(
                "Delete unsubscibed items on startup",
                Config.DeleteUnsubscribedItemsOnLoad,
                val => {
                    ConfigUtil.Config.DeleteUnsubscribedItemsOnLoad = val;
                    ConfigUtil.SaveConfig();
                }) as UICheckBox;

            button = panelHelper.AddButton("Delete Now", () => CheckSubsUtil.Instance.DeleteUnsubbed()) as UIButton;
            Settings.Pairup(checkBox, button);



            //b = g.AddButton("delete duplicates", OnPerformAllClicked) as UIButton;
            //b.tooltip = "when excluded mod is updated, and included duplicate of it is created";
        }

        static void OnPerformAllClicked() {
            Log.Debug("Perform all pressed");
            CheckSubsUtil.EnsureAll();
            SteamUtilities.DeleteUnsubbed();
        }
        static void RequestItemDetails() => CheckSubsUtil.Instance.RequestItemDetails();
    }
}
