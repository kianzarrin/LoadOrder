namespace LoadOrderTool {
    using System.IO;
    using System.Xml.Serialization;
    using CO.IO;
    using System.Linq;

    public class LoadOrderToolSettings {
        public bool AutoSave;

        const string FILE_NAME = "LoadOrderToolSettings.xml";
        static string PATH => Path.Combine(DataLocation.localApplicationData, FILE_NAME);

        static LoadOrderToolSettings instance_;
        public static LoadOrderToolSettings Instace =>
            instance_ ??= Deserialize() ?? new LoadOrderToolSettings();

        public void Serialize() {
            XmlSerializer ser = new XmlSerializer(typeof(LoadOrderToolSettings));
            using (FileStream fs = new FileStream(PATH, FileMode.Create, FileAccess.Write)) {
                ser.Serialize(fs, this);
            }
        }

        public static LoadOrderToolSettings Deserialize() {
            try {
                XmlSerializer ser = new XmlSerializer(typeof(LoadOrderToolSettings));
                using (FileStream fs = new FileStream(PATH, FileMode.Open, FileAccess.Read)) {
                    return ser.Deserialize(fs) as LoadOrderToolSettings;
                }
            } catch {
                return null;
            }
        }
    }
}
