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
        static string DIR = Path.Combine(DataLocation.localApplicationData, "LOMProfiles");

        public Mod[] Mods = new Mod[0];

        public Mod GetMod(string path) => Mods.FirstOrDefault(m => m.IncludedPath == path);

        public void Serialize(string file) {
            XmlSerializer ser = new XmlSerializer(typeof(LoadOrderProfile));
            using (FileStream fs = new FileStream(Path.Combine(DIR, file), FileMode.Create, FileAccess.Write)) {
                ser.Serialize(fs, this);
            }
        }

        public static LoadOrderProfile Deserialize(string file) {
            try {
                XmlSerializer ser = new XmlSerializer(typeof(LoadOrderProfile));
                using (FileStream fs = new FileStream(Path.Combine(DIR, file), FileMode.Open, FileAccess.Read)) {
                    return ser.Deserialize(fs) as LoadOrderProfile;
                }
            } catch {
                return null;
            }
        }
    }
}
