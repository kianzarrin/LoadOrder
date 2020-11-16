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
            foreach(var p in ColossalFramework.Plugins.PluginManager.instance.GetPluginsInfo()) {
                string savedKey = p.name + p.modPath.GetHashCode().ToString() + ".enabled";
                Log.Debug($"plugin info: savedKey={savedKey} cachedName={p.name} modPath={p.modPath}");
            }

        }

        public void OnDisabled() {
            //HarmonyUtil.UninstallHarmony(HARMONY_ID);
        }

        // public void OnSettingsUI(UIHelperBase helper) {
        //    GUI.Settings.OnSettingsUI(helper);
        // }
    }
}
