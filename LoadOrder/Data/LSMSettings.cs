using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using CO.IO;
using LoadOrderTool;

namespace LoadingScreenMod {

    [XmlRoot("LoadingScreenModRevisited")]
    public class LSMSettings {
        const string FILE_NAME = "LoadingScreenModRevisited.xml";
        static string FILE_PATH => Path.Combine(DataLocation.localApplicationData, FILE_NAME);

        public static string DefaultSkipPath => Path.Combine(DataLocation.mapLocation, "SkippedPrefabs");
        public static string DefaultSkipFile => Path.Combine(DefaultSkipPath, "skip.txt");

        public bool loadEnabled = true;
        public bool loadUsed = true;
        public bool skipPrefabs;
        public string skipFile = string.Empty;


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

        public static LSMSettings Deserialize() {
            try {
                return LoadOrderShared.SharedUtil.Deserialize<LSMSettings>( FILE_PATH);

            } catch (Exception ex) {
                ex.Log();
            }
            return new LSMSettings();
        }

        private void Serialize() {
            try {
                LoadOrderShared.SharedUtil.Serialize(this, FILE_PATH);
            } catch (Exception ex) {
                ex.Log();
            }
        }

        public LSMSettings SyncAndSerialize() {
            try {
                var ret = Deserialize();
                ret.skipFile = this.skipFile;
                ret.skipPrefabs = this.skipPrefabs;
                ret.loadEnabled = this.loadEnabled;
                ret.loadUsed = this.loadUsed;
                ret.Serialize();
                return ret;
            } catch (Exception ex) {
                ex.Log();
                return this;
            }
        }
    }
}
