namespace LoadOrderMod
{
    using System;
    using ICities;
    using KianCommons;
    using System.Diagnostics;

    public class LoadOrderMod : IUserMod {
        public static Version ModVersion => typeof(LoadOrderMod).Assembly.GetName().Version;
        public static string VersionString => ModVersion.ToString(2);
        public string Name => "Load Order Mod " + VersionString;
        public string Description => "use LoadOrderTool.exe to manage the order in which mods are loaded.";
        public static string HARMONY_ID = "CS.Kian.LoadOrder";

        public void OnEnabled() {
            Log.Debug("Testing StackTrace:\n" + new StackTrace(true).ToString(), copyToGameLog: false);
            //KianCommons.UI.TextureUtil.EmbededResources = false;
            //HelpersExtensions.VERBOSE = false;
            //HarmonyUtil.InstallHarmony(HARMONY_ID);
        }

        public void OnDisabled() {
            //HarmonyUtil.UninstallHarmony(HARMONY_ID);
        }

        // public void OnSettingsUI(UIHelperBase helper) {
        //    GUI.Settings.OnSettingsUI(helper);
        // }
    }
}
