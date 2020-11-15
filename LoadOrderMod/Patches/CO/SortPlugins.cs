using KianCommons;
using LoadOrderMod.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static ColossalFramework.Plugins.PluginManager;

namespace LoadOrderMod.Patches.CO {
    public static class SortPlugins {

        public static void Sort(Dictionary<string, PluginInfo> plugins) {
            List<KeyValuePair<string, PluginInfo>> list = new List<KeyValuePair<string, PluginInfo>>(plugins);

            for (int i = 0; i < list.Count - 1;) {
                int j = i + 1;
                if (list[i].Value.GetLoadOrder() > list[j].Value.GetLoadOrder()) {
                    var temp = list[i];
                    list[i] = list[j];
                    list[j] = temp;
                    i = 0; // restart
                } else {
                    ++i;
                }
            }

            plugins.Clear();
            foreach (var pair in list)
                plugins.Add(pair.Key, pair.Value);
        }
    }
}