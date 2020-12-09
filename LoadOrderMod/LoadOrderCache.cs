using System.IO;
using System.Xml.Serialization;

namespace LoadOrderMod {
    public class LoadOrderCache {
        public const string FILE_NAME = "LoadOrderCache.xml";
        public string WorkShopContentPath;
        public string GamePath;

        public void Serialize(string dir) {
            XmlSerializer ser = new XmlSerializer(typeof(LoadOrderCache));
            using (FileStream fs = new FileStream(Path.Combine(dir, FILE_NAME), FileMode.OpenOrCreate, FileAccess.Write)) {
                ser.Serialize(fs, this);
            }
        }
        public static LoadOrderCache Deserialize(string dir) {
            try {
                XmlSerializer ser = new XmlSerializer(typeof(LoadOrderCache));
                using (FileStream fs = new FileStream(Path.Combine(dir, FILE_NAME), FileMode.Open, FileAccess.Read)) {
                    return ser.Deserialize(fs) as LoadOrderCache;
                }
            }
            catch {
                return null;
            }
        }
    }
}
