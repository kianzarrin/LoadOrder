namespace LoadOrderTool.Util {
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Text;

    public static class ResourceUtil {
        static string AsmName => typeof(ResourceUtil).Assembly.GetName().Name;
        static string GetManifestResourcePath(string filename) => $"{AsmName}.Resources.{filename}";


        public static Icon GetIcon(string filename) {
            string path = GetManifestResourcePath(filename);
            using(var s = GetManifestResourceStream(path))
                return new Icon(s);
        }

        public static Image GetImage(string filename) {
            string path = GetManifestResourcePath(filename);
            using(var s = GetManifestResourceStream(path))
                return Image.FromStream(s);
        }

        public static Stream GetManifestResourceStream(string file) {
            try {
                return Assembly.GetExecutingAssembly().GetManifestResourceStream(file)
                    ?? throw new Exception(file + " not find");
            } catch(Exception ex) {
                Log.Exception(ex);
                throw ex;
            }
        }
    }
}
