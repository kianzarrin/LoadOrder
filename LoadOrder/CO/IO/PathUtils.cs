using System;
using System.IO;
using LoadOrderTool;
using System.Linq;

namespace CO.IO {
    public class PathUtils {
        public static string AddTimeStamp(string fileName) {
            string directoryName = Path.GetDirectoryName(fileName);
            string path = Path.GetFileNameWithoutExtension(fileName) + DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(fileName);
            return Path.Combine(directoryName, path);
        }

        public static string MakePath(string path) {
            return path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
        }

        public static string MakeUniquePath(string path) {
            string directoryName = Path.GetDirectoryName(path);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
            string extension = Path.GetExtension(path);
            int num = 1;
            while (File.Exists(path)) {
                path = Path.Combine(directoryName, string.Concat(new object[]
                {
                    fileNameWithoutExtension,
                    " ",
                    num,
                    extension
                }));
                num++;
            }
            return path;
        }

        public static string AddExtension(string path, string ext) {
            if (string.IsNullOrEmpty(Path.GetExtension(path))) {
                path += ext;
            } else {
                path = Path.ChangeExtension(path, ext);
            }
            return path;
        }

        public static string RealPath(string path) {
            if (!Directory.Exists(path) && !File.Exists(path)) {
                if(path is null) {
                    path = "<null>";
                } else if(path == "") {
                    path = "<empty>";
                }
                Log.Exception(new Exception("path not fount:" + path ?? "<null>"), showInPanel:false);
                return "";
            }
            try {
                path = Path.GetFullPath(path);
                var ret = Path.GetPathRoot(path).ToUpper();
                foreach (var name in path.Substring(ret.Length).Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries)) {
                    var entries = Directory.GetFileSystemEntries(ret);
                    ret = entries.First(
                        p => string.Equals(Path.GetFileName(p), name, StringComparison.OrdinalIgnoreCase));
                }
                //Log.Debug("returning " + root);
                return ret;
            } catch (Exception ex) {
                Log.Exception(ex, "failed to get real path for: " + path);
                return "";
            }
        }

        public static string CleanUpSlashes(string path) {
            var ar = path.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);
            return Path.Combine(ar);
        }

        public static string GoUpToDirectory(string path, string dirName) {
            try {
                if (File.Exists(path))
                    path = Path.GetDirectoryName(path);
                while (Directory.Exists(path)) {
                    if (string.Equals(Path.GetFileName(path), dirName, StringComparison.OrdinalIgnoreCase))
                        return path;
                    path = Path.GetDirectoryName(path);
                }
                return "";
            }catch(Exception ex) {
                Log.Exception(ex,$"path:{path} dirName={dirName}");
                return "";
            }
        }
    }
}
