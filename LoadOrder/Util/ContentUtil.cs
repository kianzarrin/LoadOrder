namespace LoadOrderTool.Util {
    using CO.IO;
    using CO.PlatformServices;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;

    public static class ContentUtil {
        public const string WS_URL_PREFIX = @"https://steamcommunity.com/sharedfiles/filedetails/?id=";
        public const string DLC_URL_PREFIX = @"https://store.steampowered.com/app/";

        public static string GetItemURL(PublishedFileId id) {
            if (id == PublishedFileId.invalid || id.AsUInt64 == 0)
                return null;
            return WS_URL_PREFIX + id.AsUInt64;
        }
        public static string GetItemURL(string id) {
            if (string.IsNullOrEmpty(id)) 
                return null;
            return WS_URL_PREFIX + id;
        }

        public static string GetDLCURL(PublishedFileId id) {
            if (id == PublishedFileId.invalid || id.AsUInt64 == 0)
                return null;
            return DLC_URL_PREFIX + id.AsUInt64;
        }

        public static Process OpenURL(string url) {
            try {
                var ps = new ProcessStartInfo(url) {
                    UseShellExecute = true,
                    Verb = "open"
                };
                return Process.Start(ps);
            } catch (Exception ex2) {
                Log.Exception(
                    new Exception("could not open url: " + url, ex2),
                    "could not open url");
                return null;
            }
        }

        /// <summary>
        /// opens folder or file location in explorer.
        /// </summary>
        public static Process OpenPath(string path)
        {
            Log.Called(path);
            try
            {
                if (File.Exists(path))
                {
                    string cmd = "explorer.exe";
                    string arg = "/select, " + path;
                    return Process.Start(cmd, arg);
                }
                else
                {
                    string cmd = "explorer.exe";
                    string arg = path;
                    return Process.Start(cmd, arg);
                }

            }
            catch (Exception ex)
            {
                Log.Exception(new Exception("could not open path: " + path, ex));
                return null;
            }
        }

        public static Process Execute(string dir, string exeFile, string args) {
            try {
                ProcessStartInfo startInfo = new ProcessStartInfo {
                    WorkingDirectory = dir,
                    FileName = exeFile,
                    Arguments = args,
                    WindowStyle = ProcessWindowStyle.Normal,
                    UseShellExecute = true,
                    CreateNoWindow = false,
                };
                Log.Info($"Executing ...\n" +
                    $"\tWorkingDirectory={dir}\n" +
                    $"\tFileName={exeFile}\n" +
                    $"\tArguments={args}");
                Process process = new Process { StartInfo = startInfo };
                process.Start();
                process.OutputDataReceived += (_, e) => Log.Info(e.Data);
                process.ErrorDataReceived += (_, e) => Log.Warning(e.Data);
                process.Exited += (_, e) => Log.Info("process exited with code " + process.ExitCode);
                return process;
            } catch (Exception ex) {
                Log.Exception(ex);
                return null;
            }
        }

        public static Process Subscribe(IEnumerable<PublishedFileId> ids) => Subscribe(ids.Select(id => id.ToString()));
        public static Process Subscribe(IEnumerable<ulong> ids) => Subscribe(ids.Select(id => id.ToString()));
        public static Process Subscribe(IEnumerable<string> ids) {
            if (!ids.Any()) return null;
            var ids2 = string.Join(";", ids);
            return Execute(DataLocation.GamePath, DataLocation.CitiesExe, $"--subscribe {ids2}");
        }

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


        public static bool TryGetAssetID(string path, out ulong id) {
            if (!path.Contains(DataLocation.WorkshopContentPath)) {
                id = 0;
                return false;
            }
            path = Path.GetRelativePath(DataLocation.WorkshopContentPath, path);
            int i = path.IndexOf('\\');
            var dirname = i< 0 ? path : path.Substring(0, i);
            Log.Debug($"path={path} dirname={dirname}");
            return TryGetID(dirname, out id);
        }

        public static bool TryGetModID(string dir, out ulong id) {
            string dirname = new DirectoryInfo(dir).Name;
            return TryGetID(dirname, out id);
        }

        private static bool TryGetID(string dirName, out ulong id) {
            if (dirName.StartsWith("_"))
                dirName = dirName.Remove(0, 1);
            return ulong.TryParse(dirName, out id);
        }
            

        static ulong Path2ID(string path) {
            TryGetID(Path.GetFileName(path), out ulong ret);
            return ret;
        }

        static object ensureLock_ = new object();
        public static void EnsureSubscribedItems() {
            lock (ensureLock_) {
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
                            Log.Info($"moving '{includedPath}' to '{excludedPath}'");
                            Directory.Delete(excludedPath, recursive: true);
                            Directory.Move(includedPath, excludedPath);
                            break;
                        }
                    }
                }
            }
        }

        public static void EnsureLocalItemsAt(string parentDir) {
            lock (ensureLock_) {
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
        }

        public static IEnumerable<PublishedFileId> GetSubscribedItems() {
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
