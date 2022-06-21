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
            // then built-in first, workshop second, local last
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
                    // builtin first, workshop/local-withID second, local last
                    static int order(PluginInfo _p) {
                        if (_p.isBuiltin) {
                            return 0; // built in
                        } else if (_p.publishedFileID != PublishedFileId.invalid) {
                            return 1; // WS
                        } else if (GetWSID(_p.name, out _)) {
                            return 1; // local with WS ID treat as if WS.
                        } else {
                            return 2; // local without ID
                        }
                    }
                }
                {
                    if (GetWSID(p1.name, out uint id1) && GetWSID(p2.name, out uint id2)) {
                        // if both are WS or have number in their folder name using number comparison
                        return id1.CompareTo(id2);
                    } else {
                        // if at least one has name other than a WS number then use string comparison
                        return p1.name.CompareTo(p2.name);
                    }
                }
                static bool GetWSID(string name, out uint id) {
                    return uint.TryParse(name, out id) &&
                        id != 0 &&
                        id != PublishedFileId.invalid.AsUInt64;
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

                foreach(var p in list) {

                }

                plugins.Clear();
                foreach (var pair in list)
                    plugins.Add(pair.Key, pair.Value);

                ReplaceAssembies.Init(plugins.Values.ToArray());
                Log.Info("\n=========================== plugins.Values: =======================", false);
                foreach (var p in plugins.Values) {
                    var dllFiles = Directory.GetFiles(p.modPath, "*.dll", SearchOption.AllDirectories);
                    // exclude assets.
                    if(!dllFiles.IsNullorEmpty()) {
                        string dlls = string.Join(", ", dllFiles);
                        Log.Debug(
                            $"loadOrder={p.GetLoadOrder()} path={p.modPath} dlls={{{dlls}}}"
                            , false);
                    }
                }
                Log.Info("\n=========================== END plugins.Values =====================\n", false);
            }catch(Exception ex) {
                Log.Exception(ex);
            }
        }
    }
}