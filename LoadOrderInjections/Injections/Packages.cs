namespace LoadOrderInjections.Injections {
    using System.Collections.Generic;
    using KianCommons;
    using static LoadOrderInjections.Util.LoadOrderUtil;
    public static class Packages {

        static HashSet<string> excludedPathsLowerCase_;
        static HashSet<string> Create() {
            var assets = Config?.Assets;
            var excluded = new List<string>(assets?.Length ?? 0);
            if (assets != null) {
                foreach (var item in assets) {
                    if (item.Excluded)
                        excluded.Add(item.Path.ToLower());
                }
            }
            var ret = excludedPathsLowerCase_ = new HashSet<string>(excluded);
            Log.Debug("Excluded assets = " + excludedPathsLowerCase_.JoinLines(), false);
            return ret;
        }

        // use lower case paths to work around the fact that steam might not use the right path.
        private static HashSet<string> ExcludedPathsLowerCase => excludedPathsLowerCase_ ??= Create();

        public static bool IsPathExcluded(string path) {
            return ExcludedPathsLowerCase.Contains(path.ToLower());
        }
    }
}
