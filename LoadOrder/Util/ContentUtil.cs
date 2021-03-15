namespace LoadOrderTool.Util {
    using CO.IO;
    using CO.PlatformServices;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public static class ContentUtil {
        public static bool IsPathIncluded(string fullPath) {
            return Path.GetFileName(fullPath).StartsWith("_");
        }
        public static string ToIncludedPath(string fullPath) {
            string parent = Path.GetDirectoryName(fullPath);
            string file = Path.GetFileName(fullPath);
            if(file.StartsWith("_"))
                file = file.Substring(1); //drop _
            return Path.Combine(parent, file);
        }

        public static string ToExcludedPath(string fullPath) {
            string parent = Path.GetDirectoryName(fullPath);
            string file = Path.GetFileName(fullPath);
            if (!file.StartsWith("_"))
                file = "_" + file;
            return Path.Combine(parent, file);
        }

        public static string ToIncludedPathFull(string path) {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("path");


            var dirs = SplitPath(path);
            for (int i = 0; i < dirs.Length; ++i) {
                var dir = dirs[i];
                if (dir[0] == '_')
                    dirs[i] = dir.Substring(1);
            }
            return Path.Combine(dirs);
        }

        public static string[] SplitPath(string path) {
            return path.Split(
                Path.DirectorySeparatorChar,
                StringSplitOptions.RemoveEmptyEntries);
        }

        public static bool TryGetID(string dir, out ulong id) {
            string dir2;
            if (dir.StartsWith("_"))
                dir2 = dir.Remove(0, 1);
            else
                dir2 = dir;
            return ulong.TryParse(dir2, out id);
        }

        static ulong Path2ID(string path) {
            TryGetID(Path.GetFileName(path), out ulong ret);
            return ret;
        }

        public static void EnsureSubscribedItems() {
            List<string> includedPaths = new List<string>();
            List<string> excludedPaths = new List<string>();
            foreach (var path in Directory.GetDirectories(DataLocation.WorkshopContentPath)) {
                var dirName = Path.GetFileName(path);
                if (!TryGetID(dirName, out ulong id)) continue;
                if (dirName.StartsWith("_"))
                    excludedPaths.Add(path);
                else
                    includedPaths.Add(path);
            }

            foreach (var excludedPath in excludedPaths) {
                var excludedID = Path2ID(excludedPath);
                foreach (var includedPath in includedPaths) {
                    var includedID = Path2ID(includedPath);
                    if (excludedID == includedID) {
                        Directory.Delete(excludedPath, recursive: true);
                        Directory.Move(includedPath, excludedPath);
                        break;
                    }
                }
            }
        }

        public static void EnsureLocalItemsAt(string parentDir) {
            List<string> includedPaths = new List<string>();
            List<string> excludedPaths = new List<string>();
            foreach (var path in Directory.GetDirectories(parentDir)) {
                var dirName = Path.GetFileName(path);
                if (dirName.StartsWith("_"))
                    excludedPaths.Add(path);
                else
                    includedPaths.Add(path);
            }

            foreach (var excludedPath in excludedPaths) {
                var excludedDir = Path.GetFileName(excludedPath);
                var includedDir = excludedDir.Substring(1);
                foreach (var includedPath in includedPaths) {
                    if (Path.GetFileName(includedPath) == includedDir) {
                        Directory.Delete(excludedPath, recursive: true);
                        break;
                    }
                }
            }
        }

        public static IEnumerable<PublishedFileId> GetSubscribedItems() {
            EnsureSubscribedItems();
            foreach (var path in Directory.GetDirectories(DataLocation.WorkshopContentPath)) {
                var dirName = Path.GetFileName(path);
                if (!TryGetID(dirName, out ulong id)) continue;
                //if (!Directory.GetFiles(dir, "*.dll").Any()) continue;
                yield return new PublishedFileId(id);
            }
        }

        public static string GetSubscribedItemPath(PublishedFileId id) {
            var ret = Path.Combine(DataLocation.WorkshopContentPath, id.AsUInt64.ToString());
            if (Directory.Exists(ret))
                return ret;
            ret = Path.Combine(DataLocation.WorkshopContentPath, "_" + id.AsUInt64.ToString());
            if (Directory.Exists(ret))
                return ret;
            return null;
        }
    }
}
