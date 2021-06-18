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
            Log.Debug(Environment.StackTrace);
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
            panelHelper.AddCheckbox(
                "Improve content manager",
                ConfigUtil.Config.FastContentManager,
                val => {
                    ConfigUtil.Config.FastContentManager = val;
                    ConfigUtil.SaveConfig();
                });
            panelHelper.AddCheckbox(
                "Add harmony resolver",
                ConfigUtil.Config.AddHarmonyResolver,
                val => {
                    ConfigUtil.Config.AddHarmonyResolver = val;
                    ConfigUtil.SaveConfig();
                });
        }

        static void OnResetLoadOrdersClicked() {
            Log.Debug("Reset Load Orders pressed");
            foreach (var mod in Config.Mods)
                mod.LoadOrder = LoadOrderConfig.DefaultLoadOrder;
            ConfigUtil.SaveConfig();

        }
    }
}
