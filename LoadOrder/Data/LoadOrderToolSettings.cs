namespace LoadOrderTool {
    using System.IO;
    using System.Xml.Serialization;
    using CO.IO;
    using System.Linq;
    using System.Windows.Forms;
    using System.Drawing;

    public class LoadOrderToolSettings {
        public int FormWidth = -1;
        public int FormHeight = -1;

        public bool AutoSave;
        public string SavedGamePath, MapPath;

        public bool NoAssets, NoMods, NoWorkshop;
        public bool NewAsset;
        public bool LSM = true;
        public bool LHT;
        public int AutoLoad = 0;
        public bool Phased, Poke;

        public bool SteamExe = true;
        public bool DebugMono;

        public string ExtraArgs;


        const string FILE_NAME = "LoadOrderToolSettings.xml";
        static string PATH => Path.Combine(DataLocation.localApplicationData, FILE_NAME);

        static LoadOrderToolSettings instance_;
        public static LoadOrderToolSettings Instace =>
            instance_ ??= Deserialize() ?? new LoadOrderToolSettings();

        public static void Reset() => instance_ = new LoadOrderToolSettings();

        public void Serialize() {
            XmlSerializer ser = new XmlSerializer(typeof(LoadOrderToolSettings));
            using (FileStream fs = new FileStream(PATH, FileMode.Create, FileAccess.Write)) {
                ser.Serialize(fs, this);
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
                Log.Warning("Deserialize exception catched");
                return null;
            } finally {

            }
        }
    }
}
