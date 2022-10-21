namespace LoadOrderShared {
    using System.IO;
    using System.Xml.Serialization;
    using System.Xml;

    internal class SharedUtil {
#if TOOL
        internal static XmlSerializerNamespaces NoNamespaces {
            get {
                var ret = new XmlSerializerNamespaces();
                ret.Add("", "");
                return ret;
            }
        }

        internal static XmlWriterSettings Indented => new XmlWriterSettings() { Indent = true };
        internal static void Serialize<T>(T obj, string filePath) {
            var serializer = new XmlSerializer(typeof(T));
            using (StreamWriter writer = new StreamWriter(filePath)) {
                using (var xmlWriter = XmlWriter.Create(writer, Indented)) {
                    xmlWriter.Settings.Indent = true;
                    serializer.Serialize(xmlWriter, obj, NoNamespaces);
                }
            }
        }

        internal static string LocalApplicationData => CO.IO.DataLocation.localApplicationData;
        internal static string LocalLOMData => CO.IO.DataLocation.LocalLOMData;
#else
#endif
        public static T Deserialize<T>(string filePath) where T : class {
            try {
                XmlSerializer ser = new XmlSerializer(typeof(T));
                var dirInfo = new FileInfo(filePath).Directory;
                if (dirInfo.Exists) dirInfo.Create();
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read)) {
                    return ser.Deserialize(fs) as T;
                }
            } catch {
                return null;
            }
        }

    }
}
