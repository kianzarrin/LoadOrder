namespace LoadOrder.Util {
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;

    public static class XMLUtil {
        public static XmlSerializerNamespaces NoNamespaces {
            get {
                var ret = new XmlSerializerNamespaces();
                ret.Add("", "");
                return ret;
            }
        }

        public static XmlWriterSettings Indented => new XmlWriterSettings() { Indent = true };
        public static void Serialize<T>(T obj, string filePath) {
                var serializer = new XmlSerializer(typeof(T));
                using (StreamWriter writer = new StreamWriter(filePath)) {
                    using (var xmlWriter = XmlWriter.Create(writer, Indented)) {
                        xmlWriter.Settings.Indent = true;
                        serializer.Serialize(xmlWriter, obj, NoNamespaces);
                    }
                }
        }
    }
}
