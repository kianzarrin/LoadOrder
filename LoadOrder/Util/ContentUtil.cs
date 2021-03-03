namespace LoadOrderTool.Util {
    using CO.IO;
    using CO.PlatformServices;
    using System.Collections.Generic;
    using System.IO;

    public static class ContentUtil {
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
                var excludedID = Path2ID(excludedPath);
                foreach (var includedPath in includedPaths) {
                    var includedID = Path2ID(includedPath);
                    if (excludedID == includedID) {
                        Directory.Delete(excludedPath, recursive: true);
                        //Directory.Move(includedPath, excludedPath);
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
