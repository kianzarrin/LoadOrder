namespace LoadOrderInjections.Injections {
    using System.Collections.Generic;
    using static LoadOrderInjections.Util.LoadOrderUtil;
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
