using CO.IO;
using LoadOrder.Util;
using LoadOrderTool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

using System;
using System.IO;
using System.Xml;

namespace LoadingScreenMod {
    public class LSMSettings {
        const string ROOT = "LoadingScreenModRevisited";
        const string LEGACY_ROOT = "Settings";
        const string FILE_NAME_LEGACY = "LoadingScreenMod.xml";
        const string FILE_NAME_REVISITED = "LoadingScreenModRevisited.xml";
        static string FilePathLegacy => Path.Combine(DataLocation.GamePath, FILE_NAME_LEGACY);
        static string FilePathRevisited => Path.Combine(DataLocation.localApplicationData, FILE_NAME_REVISITED);

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
            LSMSettings ret = null;
            try {
                DateTime tLegacy = default;
                DateTime tRevisited = default;
                if (File.Exists(FilePathLegacy)) {
                    tLegacy = File.GetLastWriteTimeUtc(FilePathLegacy);
                }
                if (File.Exists(FilePathRevisited)) {
                    tRevisited = File.GetLastWriteTimeUtc(FilePathRevisited);
                }

                if (tLegacy > tRevisited) {
                    ret = Deserialize(FilePathLegacy, LEGACY_ROOT);
                } else if (tRevisited != default) {
                    ret = Deserialize(FilePathRevisited,ROOT);
                }
            } catch (Exception ex) {
                ex.Log();
            }
            ret ??= new LSMSettings();
            return ret;
        }
        public static LSMSettings Deserialize(string file, string root) {
            try {
                if (File.Exists(file)) {
                    XmlSerializer serializer = new XmlSerializer(typeof(LSMSettings), root: new XmlRootAttribute(root));
                    using (StreamReader reader = new StreamReader(file))
                        return (LSMSettings)serializer.Deserialize(reader);
                }
            } catch (Exception ex) {
                ex.Log();
            }
            return null;
        }

        public void Serialize() {
            if (File.Exists(FilePathLegacy) || !File.Exists(FilePathRevisited)) {
                Serialize(FilePathLegacy, LEGACY_ROOT);
            }
            Serialize(FilePathRevisited, ROOT);
        }

        private void Serialize(string file, string root) {
            try {
                var serializer = new XmlSerializer(typeof(LSMSettings), root: new XmlRootAttribute(root));
                using (StreamWriter tw = new StreamWriter(file)) {
                    using (XmlTextWriter xmlw = new XmlTextWriter(tw)) {
                        xmlw.Formatting = Formatting.Indented;
                        xmlw.Indentation = 2;
                        serializer.Serialize(xmlw, this, XMLUtil.NoNamespaces);
                    }
                }
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
