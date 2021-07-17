namespace LoadOrderMod.Settings.Tabs {
    extern alias Injections;
    using ColossalFramework.PlatformServices;
    using ColossalFramework.UI;
    using KianCommons;
    using LoadOrderShared;
    using System;
    using UnityEngine;
    using SteamUtilities = Injections.LoadOrderInjections.SteamUtilities;
    using LoadOrderMod.UI;
    using KianCommons.UI;
    static class StartupTab {
        static LoadOrderConfig Config => ConfigUtil.Config;

        public static void Make(ExtUITabstrip tabStrip) {
            UIHelper panelHelper = tabStrip.AddTabPage("Startup");
            panelHelper.AddLabel("restart required to take effect.", textColor: Color.yellow);
            panelHelper.AddSpace(10);

            panelHelper.AddButton("Reset load orders", OnResetLoadOrdersClicked);

            panelHelper.AddCheckbox(
                "remove ad panels",
                ConfigUtil.Config.TurnOffSteamPanels,
                val => {
                    ConfigUtil.Config.TurnOffSteamPanels = val;
                    ConfigUtil.SaveConfig();
                });
            var c2 = panelHelper.AddCheckbox(
                "Improve content manager",
                ConfigUtil.Config.FastContentManager,
                val => {
                    ConfigUtil.Config.FastContentManager = val;
                    ConfigUtil.SaveConfig();
                }) as UIComponent;
            c2.tooltip = "faster content manager";
            var c3 = panelHelper.AddCheckbox(
                "Add harmony resolver",
                ConfigUtil.Config.AddHarmonyResolver,
                val => {
                    ConfigUtil.Config.AddHarmonyResolver = val;
                    ConfigUtil.SaveConfig();
                }) as UICheckBox;
        }

        static void OnResetLoadOrdersClicked() {
            Log.Debug("Reset Load Orders pressed");
            foreach (var mod in Config.Mods)
                mod.LoadOrder = LoadOrderConfig.DefaultLoadOrder;
            ConfigUtil.SaveConfig();

        }
    }
}