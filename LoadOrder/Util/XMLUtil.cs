namespace LoadOrder.Util {
    using System.Xml.Serialization;

    public static class XMLUtil {
        public static XmlSerializerNamespaces NoNamespaces {
            get {
                var ret = new XmlSerializerNamespaces();
                ret.Add("", "");
                return ret;
            }
        }
    }
}
