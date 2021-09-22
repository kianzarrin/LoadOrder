namespace LoadOrderInjections.Injections {
    using System.Collections.Generic;
    using KianCommons;
    using static LoadOrderInjections.Util.LoadOrderUtil;
    public static class Packages {
        static HashSet<string> excludedPaths_;
        static HashSet<string> Create() {
            var assets = Config?.Assets;
            var excluded = new List<string>(assets?.Length ?? 0);
            if (assets != null) {
                foreach (var item in assets) {
                    if (item.Excluded)
                        excluded.Add(item.Path);
                }
            }
            var ret = excludedPaths_ = new HashSet<string>(excluded);
            Log.Debug("Excluded assets = " + excludedPaths_.JoinLines(), false);
            return ret;
        }
        static HashSet<string> ExcludedPaths => excludedPaths_ ??= Create();
        public static bool IsPathExcluded(string path) {
            return ExcludedPaths.Contains(path);
        }
    }
}
