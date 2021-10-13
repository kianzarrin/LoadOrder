namespace LoadOrderIPatch {
    using System.Collections.Generic;

    public static class Packages {

        static HashSet<string> excludedPathsLowerCase_;
        static HashSet<string> Create() {
            var assets = ConfigUtil.Config?.Assets;
            var excluded = new List<string>(assets?.Length ?? 0);
            if (assets != null) {
                foreach (var item in assets) {
                    if (item.Excluded)
                        excluded.Add(item.Path.ToLower());
                }
            }
            var ret = excludedPathsLowerCase_ = new HashSet<string>(excluded);
            return ret;
        }

        // use lower case paths to work around the fact that steam might not use the right path.
        private static HashSet<string> ExcludedPathsLowerCase => excludedPathsLowerCase_ ??= Create();

        public static bool IsFileExcluded(string path) {
            if (string.IsNullOrEmpty(path) || path[0] == '_')
                return true;
            return Packages.IsPathExcluded(path);
        }

        public static bool IsPathExcluded(string path) {
            return ExcludedPathsLowerCase.Contains(path.ToLower());
        }
    }
}
