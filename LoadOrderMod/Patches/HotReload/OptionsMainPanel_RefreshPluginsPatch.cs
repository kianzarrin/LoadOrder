namespace LoadOrderMod.Patches.HotReload {
    using HarmonyLib;
    using System;
    using static KianCommons.ReflectionHelpers;
    using ColossalFramework.Plugins;
    using System.Collections.Generic;
    using System.Reflection.Emit;
    using static ColossalFramework.Plugins.PluginManager;
    using System.Reflection;
    using KianCommons.Patches;
    using System.Diagnostics;
    using KianCommons;
    using LoadOrderMod.Util;
    using System.Linq;

    /// <summary>
    /// time each invocation.
    /// </summary>
    [HarmonyPatch(typeof(OptionsMainPanel), "RefreshPlugins")]
    public static class OptionsMainPanel_RefreshPluginsPatch {

        static bool Prefix() {
            if (RemovePluginAtPathPatch.name != null) {
                HotReloadUtil.DropCategory(RemovePluginAtPathPatch.name);
                return false;
            } else if (LoadPluginAtPathPatch.name != null ) {
                var p = PluginManager.instance.GetPluginsInfo().FirstOrDefault(
                    item => item.isEnabled && item.name == LoadPluginAtPathPatch.name);
                if (p != null)
                    HotReloadUtil.AddCategory(p);
                return false;
            } else {
                return true; //proceed as normal.
            }
        }
    }
}
