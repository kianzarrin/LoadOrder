using ColossalFramework.Plugins;
using LoadOrderMod.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static ColossalFramework.Plugins.PluginManager;
using KianCommons;

namespace LoadOrderMod.Patches.CO {
    public static class AddPluginsPatch {
        static Dictionary<string, PluginInfo> GetPlugins() =>
            typeof(PluginManager).GetField("m_Plugins").GetValue(PluginManager.instance) as Dictionary<string, PluginInfo>;

        static MethodInfo mTriggerEventPluginsChanged =
            typeof(PluginManager).
            GetMethod("TriggerEventPluginsChanged", BindingFlags.Instance | BindingFlags.NonPublic) ??
            throw new Exception("TriggerEventPluginsChanged not found");

        static void TriggerEventPluginsChanged() =>
            mTriggerEventPluginsChanged.Invoke(PluginManager.instance, null);

        public static void AddPlugginsSorted(Dictionary<string, PluginInfo> plugins) {
            var keys = plugins.Keys.ToList();
            int SortByLoadOrder(string path1, string path2) {
                int _i1 = plugins[path1].GetLoadOrder();
                int _i2 = plugins[path2].GetLoadOrder();
                return _i1.CompareTo(_i2);
            }
            keys.Sort(SortByLoadOrder);

            var m_Plugins = GetPlugins();
            foreach (string key in keys) {
                PluginInfo pluginInfo = plugins[key];
                if (pluginInfo.assemblyCount > 0) {
                    PluginInfo pluginInfo2;
                    if (m_Plugins.TryGetValue(key, out pluginInfo2)) {
                        pluginInfo2.Unload();
                        pluginInfo2 = null;
                        m_Plugins[key] = pluginInfo;
                    } else {
                        m_Plugins.Add(key, pluginInfo);
                    }
                    if (pluginInfo.isEnabled) {
                        if (pluginInfo.userModInstance != null) {
                            MethodInfo method = pluginInfo.userModInstance.GetType()
                                .GetMethod("OnEnabled", BindingFlags.Instance | BindingFlags.Public);
                            if (method != null) {
                                try {
                                    method.Invoke(pluginInfo.userModInstance, null);
                                }
                                catch (Exception ex) {
                                    Log.Exception(ex);
                                }
                            }
                        } else {
                            Log.Error(pluginInfo.name + " is not instantiated. This should never happen.");
                        }
                    }
                    TriggerEventPluginsChanged();
                }
            }
        }
    }
}