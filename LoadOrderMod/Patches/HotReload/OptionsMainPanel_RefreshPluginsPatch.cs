namespace LoadOrderMod.Patches.HotReload {
    using HarmonyLib;
    using System;
    using ColossalFramework.Plugins;
    using KianCommons;
    using LoadOrderMod.Util;
    using System.Linq;
    using LoadOrderMod.UI;

    /// <summary>
    /// time each invocation.
    /// </summary>
    [HarmonyPatch(typeof(OptionsMainPanel), "RefreshPlugins")]
    public static class OptionsMainPanel_RefreshPluginsPatch {

        static bool Prefix(OptionsMainPanel __instance) {
            try {
                if (RemovePluginAtPathPatch.name != null) {
                    __instance.DropCategory(RemovePluginAtPathPatch.name);
                    StatusUtil.Instance?.ModUnloaded();
                    return false;
                } else if (LoadPluginAtPathPatch.name != null) {
                    var p = PluginManager.instance.GetPluginsInfo().FirstOrDefault(
                        item => item.isEnabled && item.name == LoadPluginAtPathPatch.name);
                    if (p != null)
                        __instance.AddCategory(p);
                    StatusUtil.Instance?.ModLoaded();
                    return false;
                } else {
                    return true; //proceed as normal.
                }
            } catch (Exception ex) {
                Log.Exception(ex);
                return true;
            }
        }
    }
}
