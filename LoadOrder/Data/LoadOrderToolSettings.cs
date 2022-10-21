namespace LoadOrderTool {
    using System.IO;
    using System.Xml.Serialization;
    using CO.IO;

    public class LoadOrderToolSettings {
        public int FormWidth = -1;
        public int FormHeight = -1;

        public bool AutoSave = true;
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
            LoadOrderShared.SharedUtil.Serialize(this, PATH);
        }

        public static LoadOrderToolSettings Deserialize() {
            Log.Info("LoadOrderToolSettings Deserializing ...");
            try {
                return LoadOrderShared.SharedUtil.Deserialize<LoadOrderToolSettings>(PATH);
            } catch {
                Log.Warning("Deserialize exception caught");
                return null;
            } finally {

            }
        }
    }
}
