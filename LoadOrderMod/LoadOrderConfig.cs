using System.IO;
using System.Xml.Serialization;

namespace LoadOrderShared {
    public class ModInfo {
        public string Path; // included path
        public int LoadOrder;
        public string AssemblyName;
        public string ModName;
        public string Description;
    }

    public class AssetInfo {
        public string Path; // included path
        public string AssetName;
        public string description;
        public string Date;
        public string[] Tags;
        public string Author;

    }

    public class LoadOrderConfig {
        public const int DefaultLoadOrder = 1000;
        public const string FILE_NAME = "LoadOrderConfig.xml";

        public string WorkShopContentPath;
        public string GamePath;
        public ModInfo[] Mods = new ModInfo[0];
        public AssetInfo[] Assets = new AssetInfo[0];

        public void Serialize(string dir) {
            XmlSerializer ser = new XmlSerializer(typeof(LoadOrderConfig));
            using (FileStream fs = new FileStream(Path.Combine(dir, FILE_NAME), FileMode.Create, FileAccess.Write)) {
                ser.Serialize(fs, this);
            }
        }
        
        public static LoadOrderConfig Deserialize(string dir) {
            try {
                XmlSerializer ser = new XmlSerializer(typeof(LoadOrderConfig));
                using (FileStream fs = new FileStream(Path.Combine(dir, FILE_NAME), FileMode.Open, FileAccess.Read)) {
                    return ser.Deserialize(fs) as LoadOrderConfig;
                }
            }
            catch {
                return null;
            }
        }
    }
}
