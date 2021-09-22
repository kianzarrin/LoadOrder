using System;
using System.IO;
using System.Xml.Serialization;
using CO.IO;
using LoadOrderTool;
using LoadOrderTool.Util;

namespace LoadingScreenMod {
    public class Settings {
        const string FILENAME = "LoadingScreenMod.xml";
        static string FilePath => Path.Combine(DataLocation.GamePath, FILENAME);
        public static string DefaultSkipPath => Path.Combine(DataLocation.mapLocation, "SkippedPrefabs");
        public static string DefaultSkipFile => Path.Combine(DefaultSkipPath, "skip.txt");

        public int version = 10;
        public bool loadEnabled = true;
        public bool loadUsed = true;
        public bool shareTextures = true;
        public bool shareMaterials = true;
        public bool shareMeshes = true;
        public bool optimizeThumbs = true;
        public bool reportAssets;
        public bool checkAssets;
        public string reportDir = string.Empty;
        public bool skipPrefabs;
        public string skipFile = string.Empty;
        public bool hideAssets;
        public bool useReportDate = true;


        public static Settings Deserialize() {
            Settings s;

            try {
                XmlSerializer serializer = new XmlSerializer(typeof(Settings));

                using (StreamReader reader = new StreamReader(FilePath))
                    s = (Settings)serializer.Deserialize(reader);
            } catch (Exception) { s = new Settings(); }

            s.version = 6;
            return s;
        }

        public void Serialize() {
            try {
                XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                using (StreamWriter writer = new StreamWriter(FilePath))
                    serializer.Serialize(writer, this);
            } catch (Exception ex) {
                ex.Log();
            }
        }

        public Settings SyncAndSerialize() {
            try {
                var ret = Deserialize();
                ret.skipFile = this.skipFile;
                ret.skipPrefabs = this.skipPrefabs;
                ret.Serialize();
                return ret;
            } catch(Exception ex) {
                ex.Log();
                return this;
            }
        }

    }
}
