using KianCommons;
using LoadOrderMod.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static ColossalFramework.Plugins.PluginManager;
using  ColossalFramework.PlatformServices;

namespace LoadOrderMod.Patches.CO {
    public static class SortPlugins {

        public static int Comparison(PluginInfo p1, PluginInfo p2) {
            var savedOrder1 = p1.SavedLoadOrder();
            var savedOrder2 = p2.SavedLoadOrder();

            // orderless harmony comes first
            if (!savedOrder1.exists && p1.IsHarmonyMod())
                return -1;
            if (!savedOrder2.exists && p2.IsHarmonyMod())
                return +1;

            // if neither have order, use string comparison
            // then builin first, workshop second, local last
            // otherwise use string comparison
            if (!savedOrder1.exists && !savedOrder2.exists) {
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

        public static void Sort(Dictionary<string, PluginInfo> plugins) {
            var list = plugins.ToList();
            list.Sort((p1, p2) => Comparison(p1.Value, p2.Value));
            plugins.Clear();
            foreach (var pair in list)
                plugins.Add(pair.Key, pair.Value);
        }
    }
}