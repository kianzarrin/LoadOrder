namespace LoadOrderTool {
    using System.IO;
    using System.Xml.Serialization;
    using CO.IO;
    using LoadOrder.Util;

    public class LoadOrderToolSettings {
        public int FormWidth = -1;
        public int FormHeight = -1;

        public bool AutoSync = true;
        public bool Advanced = false;
        public string SavedGamePath, MapPath;

        public bool NoAssets, NoMods, NoWorkshop, ResetAssets;
        public bool NewAsset;
        public bool LSM = false;
        public bool LHT;
        public int AutoLoad = 0;
        public bool Phased, Poke;

        public bool SteamExe = true;
        public bool DebugMono;
        public bool ProfilerCities;

        public string ExtraArgs;

        public string LastProfileName;


        const string FILE_NAME = "LoadOrderToolSettings.xml";
        static string PATH => Path.Combine(DataLocation.LocalLOMData, FILE_NAME);

        static LoadOrderToolSettings instance_;
        public static LoadOrderToolSettings Instace =>
            instance_ ??= Deserialize() ?? new LoadOrderToolSettings();

        public static void Reset() => instance_ = new LoadOrderToolSettings();

        public void Serialize() {
            XmlSerializer ser = new XmlSerializer(typeof(LoadOrderToolSettings));
            using (FileStream fs = new FileStream(PATH, FileMode.Create, FileAccess.Write)) {
                ser.Serialize(fs, this, XMLUtil.NoNamespaces);
            }
        }

        public static LoadOrderToolSettings Deserialize() {
            Log.Info("LoadOrderToolSettings Deserializing ...");
            try {
                XmlSerializer ser = new XmlSerializer(typeof(LoadOrderToolSettings));
                using (FileStream fs = new FileStream(PATH, FileMode.Open, FileAccess.Read)) {
                    return ser.Deserialize(fs) as LoadOrderToolSettings;
                }
            } catch {
                Log.Warning("Deserialize exception caught");
                return null;
            } finally {

            }
        }
    }
}
