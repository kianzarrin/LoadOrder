namespace LoadOrderInjections.Injections {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using ColossalFramework.Plugins;
    using static ColossalFramework.Plugins.PluginManager;
    using KianCommons;
    using static KianCommons.ReflectionHelpers;
    using System.Runtime.CompilerServices;
    using ICities;
    using static LoadOrderInjections.Util.LoadOrderUtil;
    using Mono.Cecil;
    using LoadOrderInjections.Util;
    public static class Packages {
        static HashSet<string> excludedPaths_;
        static HashSet<string> Create() {
            excludedPaths_ = new HashSet<string>();
            if (Config?.Assets != null) {
                foreach (var item in Config.Assets) {
                    if (item.Excluded)
                        excludedPaths_.Add(item.Path);
                }
            }
            return excludedPaths_;
        }
        static HashSet<string> ExcludedPaths => excludedPaths_ ??= Create();
        public static bool IsPathExcluded(string path) {
            return ExcludedPaths.Contains(path);
            //Log.Debug($"searching path in:\n" + ExcludedPaths.JoinLines());
            //return ExcludedPaths.Contains(path).LogRet($"IsPathExcluded({path})");
        }
    }
}
