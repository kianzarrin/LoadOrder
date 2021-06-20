namespace LoadOrderInjections.Injections {
    using System.Collections.Generic;
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
            return excludedPaths_ = new HashSet<string>(excluded);
        }
        static HashSet<string> ExcludedPaths => excludedPaths_ ??= Create();
        public static bool IsPathExcluded(string path) {
            return ExcludedPaths.Contains(path);
            //Log.Debug($"searching path in:\n" + ExcludedPaths.JoinLines());
            //return ExcludedPaths.Contains(path).LogRet($"IsPathExcluded({path})");
        }
    }
}
