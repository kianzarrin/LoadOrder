using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using CO.IO;
using LoadOrder.Util;
using LoadOrderTool;

namespace LoadingScreenMod {
    public class Settings {
        const string FILENAME = "LoadingScreenMod.xml";
        static string FilePath => Path.Combine(DataLocation.GamePath, FILENAME);
        public static string DefaultSkipPath => Path.Combine(DataLocation.mapLocation, "SkippedPrefabs");
        public static string DefaultSkipFile => Path.Combine(DefaultSkipPath, "skip.txt");

        //public int version = 10;
        public bool loadEnabled = true;
        public bool loadUsed = true;
        //public bool shareTextures = true;
        //public bool shareMaterials = true;
        //public bool shareMeshes = true;
        //public bool optimizeThumbs = true;
        //public bool reportAssets;
        //public bool checkAssets;
        //public string reportDir = string.Empty;
        public bool skipPrefabs;
        public string skipFile = string.Empty;
        //public bool hideAssets;
        //public bool useReportDate = true;

        #region rest of elements
        private readonly List<XElement> elements = new List<XElement>();
        [XmlAnyElement]
        public List<XElement> Elements => elements;

        public string this[XName name] {
            get {
                return Elements.Where(e => e.Name == name).Select(e => e.Value).FirstOrDefault();
            }
            set {
                var element = Elements.Where(e => e.Name == name).FirstOrDefault();
                if (element == null)
                    Elements.Add(element = new XElement(name));
                element.Value = value;
            }
        }

        public override string ToString() {
            var ret = $"loadEnabled={loadEnabled}, loadUsed={loadUsed}, skipPrefabs={skipPrefabs}, skipFile={skipFile}";
            foreach (var item in Elements) {
                ret += $", {item.Name}={item.Value}";
            }
            return ret;
        }
        #endregion

        public static Settings Deserialize() {
            Settings ret;

            try {
                XmlSerializer serializer = new XmlSerializer(typeof(Settings));

                using (StreamReader reader = new StreamReader(FilePath))
                    ret = (Settings)serializer.Deserialize(reader);
            } catch (Exception) { ret = new Settings(); }
            return ret;
        }

        public void Serialize() {
            try {
                XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                using (StreamWriter writer = new StreamWriter(FilePath))
                    serializer.Serialize(writer, this, XMLUtil.NoNamespaces);
            } catch (Exception ex) {
                ex.Log();
            }
        }

        public Settings SyncAndSerialize() {
            try {
                var ret = Deserialize();
                ret.skipFile = this.skipFile;
                ret.skipPrefabs = this.skipPrefabs;
                ret.loadEnabled = this.loadEnabled;
                ret.loadUsed = this.loadUsed;
                ret.Serialize();
                return ret;
            } catch(Exception ex) {
                ex.Log();
                return this;
            }
        }

    }
}
