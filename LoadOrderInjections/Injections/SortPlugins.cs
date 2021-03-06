using ColossalFramework.PlatformServices;
using KianCommons;
using LoadOrderInjections.Util;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using PluginInfo = ColossalFramework.Plugins.PluginManager.PluginInfo;
using System;

namespace LoadOrderInjections.Injections {
    public static class SortPlugins {
        const int DefaultLoadOrder = LoadOrderShared.LoadOrderConfig.DefaultLoadOrder;

        public static int OldComparison(PluginInfo p1, PluginInfo p2) {
            var savedOrder1 = p1.GetLoadOrder();
            var savedOrder2 = p2.GetLoadOrder();

            // orderless harmony comes first
            if (!p1.HasLoadOrder() && p1.IsHarmonyMod())
                return -1;
            if (!p2.HasLoadOrder() && p2.IsHarmonyMod())
                return +1;

            // if neither have order, use string comparison
            // then builin first, workshop second, local last
            // otherwise use string comparison
            if (!p1.HasLoadOrder() && !p2.HasLoadOrder()) {
                int order(PluginInfo _p) =>
                    _p.isBuiltin ? 0 :
                    (_p.publishedFileID != PublishedFileId.invalid ? 1 : 2);
                if (order(p1) != order(p2))
                    return order(p1) - order(p2);
                return p1.name.CompareTo(p2.name);
            }

            // use assigned or default values
            return savedOrder1 - savedOrder2;
        }

        /// <returns>
        /// harmony mod : 2.0.1.0+
        /// harmony 2 : 2.0.0.0
        /// harmony 1 : 1.*
        /// no harmony : 0
        /// </returns>
        public static int GetHarmonyOrder(PluginInfo p) {
            if (p.IsHarmonyMod())
                return 0;
            foreach (var asm in p.GetAssemblies()) {
                var name = asm.GetName().Name;
                if (name == "CitiesHarmony.API")
                    return 1;
                if (name == "0Harmony")
                    return 2;
            }
            return 3;
        }

        public static int HarmonyComparison(PluginInfo p1, PluginInfo p2) {
            var savedOrder1 = p1.GetLoadOrder();
            var savedOrder2 = p2.GetLoadOrder();

            // orderless harmony comes first
            if (!p1.HasLoadOrder() && p1.IsHarmonyMod())
                return -1;
            if (!p2.HasLoadOrder() && p2.IsHarmonyMod())
                return +1;

            if (!p1.HasLoadOrder() && !p2.HasLoadOrder()) {
                // if neither have saved order,
                {
                    // 1st: harmony mod 
                    // 2nd: harmony 2 
                    // 3rd: harmony 1 
                    // 4th: no harmony 
                    var o1 = GetHarmonyOrder(p1);
                    var o2 = GetHarmonyOrder(p2);
                    if (o1 != o2)
                        return o1 - o2;
                }
                {
                    // builtin first, workshop second, local last
                    static int order(PluginInfo _p) =>
                        _p.isBuiltin ? 0 :
                        (_p.publishedFileID != PublishedFileId.invalid ? 1 : 2);
                    if (order(p1) != order(p2))
                        return order(p1) - order(p2);
                }
                {
                    // use string comparison
                    return p1.name.CompareTo(p2.name);
                }
            }

            // use assigned or default values
            return savedOrder1 - savedOrder2;
        }


        public static void Sort(Dictionary<string, PluginInfo> plugins) {
            try {
                var list = plugins.ToList();

                Log.Info("Sorting assemblies ...", true);
                list.Sort((p1, p2) => HarmonyComparison(p1.Value, p2.Value));

                plugins.Clear();
                foreach (var pair in list)
                    plugins.Add(pair.Key, pair.Value);

                Log.Info("\n=========================== plugins.Values: =======================", false);
                foreach (var p in plugins.Values) {
                    string dlls = string.Join(", ",
                        Directory.GetFiles(p.modPath, "*.dll", SearchOption.AllDirectories));
                    Log.Debug(
                        $"loadOrder={p.GetLoadOrder()} path={p.modPath} dlls={{{dlls}}}"
                        , false);
                }
                Log.Info("\n=========================== END plugins.Values =====================\n", false);
            }catch(Exception ex) {
                Log.Exception(ex);
            }
        }
    }
}