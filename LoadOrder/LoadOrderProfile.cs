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
                IsIncluded = pluginInfo.IsIncluded;
                IsEnabled = pluginInfo.isEnabled;
                LoadOrder = pluginInfo.LoadOrder;
                DisplayText = pluginInfo.DisplayText;
            }

            public void Write(CO.Plugins.PluginManager.PluginInfo pluginInfo) {
                pluginInfo.IsIncluded = IsIncluded;
                pluginInfo.isEnabled = IsEnabled;
                pluginInfo.LoadOrder = LoadOrder;
            }



        }

        const int DefaultLoadOrder = LoadOrderShared.LoadOrderConfig.DefaultLoadOrder;
        public static string DIR {
            get {
                var dir = Path.Combine(DataLocation.localApplicationData, "LOMProfiles");
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                return dir;
            }
        }

        public Mod[] Mods = new Mod[0];

        public Mod GetMod(string path) => Mods.FirstOrDefault(m => m.IncludedPath == path);

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
