namespace LoadOrderTool {
    using System.IO;
    using System.Xml.Serialization;
    using CO.IO;
    using System.Linq;

    public class LoadOrderProfile {
        public class Mod {
            public string IncludedPath;
            public bool IsEnabled;
            public bool IsIncluded;
            public int LoadOrder;
            public string DisplayText;
            public Mod() { }

            public Mod(CO.Plugins.PluginManager.PluginInfo pluginInfo) {
                IncludedPath = pluginInfo.ModIncludedPath;
                IsIncluded = pluginInfo.IsIncludedPending;
                IsEnabled = pluginInfo.IsEnabledPending;
                LoadOrder = pluginInfo.LoadOrder;
                DisplayText = pluginInfo.DisplayText;
            }

            public void Write(CO.Plugins.PluginManager.PluginInfo pluginInfo) {
                pluginInfo.IsIncludedPending = IsIncluded;
                pluginInfo.IsEnabledPending = IsEnabled;
                pluginInfo.LoadOrder = LoadOrder;
            }
        }

        public class Asset {
            public string IncludedPath;
            public bool IsIncluded;
            public string DisplayText;

            public Asset() { }

            public Asset(CO.Packaging.PackageManager.AssetInfo assetInfo) {
                IncludedPath = assetInfo.AssetPath;
                IsIncluded = assetInfo.IsIncludedPending;
                DisplayText = assetInfo.DisplayText;
            }

            public void Write(CO.Packaging.PackageManager.AssetInfo assetInfo) {
                assetInfo.IsIncludedPending = IsIncluded;
            }
        }


        public static string DIR {
            get {
                var dir = Path.Combine(DataLocation.localApplicationData, "LOMProfiles");
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                return dir;
            }
        }

        public Mod[] Mods = new Mod[0];
        public Asset[] Assets = new Asset[0];

        public Mod GetMod(string includedPath) => Mods.FirstOrDefault(m => m.IncludedPath == includedPath);
        public Asset GetAsset(string includedPath) => Assets.FirstOrDefault(m => m.IncludedPath == includedPath);

        public void Serialize(string path) {
            XmlSerializer ser = new XmlSerializer(typeof(LoadOrderProfile));
            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write)) {
                ser.Serialize(fs, this);
            }
        }

        public static LoadOrderProfile Deserialize(string path) {
            try {
                XmlSerializer ser = new XmlSerializer(typeof(LoadOrderProfile));
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read)) {
                    return ser.Deserialize(fs) as LoadOrderProfile;
                }
            } catch {
                return null;
            }
        }
    }
}
