namespace LoadOrderShared {
    using System.IO;
    using System.Xml.Serialization;
    using System.Collections.Generic;
    using System.Collections;

    public class CSCache {
        public class Item {
            public string IncludedPath;
            public string Name;
            public string Description;
        }

        public class Mod: Item {
        }

        public class Asset: Item {
            public string[] Tags;
        }

        public const string FILE_NAME = "CSCache.xml";

        public string WorkShopContentPath;
        public string GamePath;
        public string SteamPath;


        public List<Item> Items = new List<Item>(200000);

        internal Hashtable ItemTable = new Hashtable(100000);

        public void AddItem(Item item) {
            Items.Add(item);
            ItemTable[item.IncludedPath] = item;
        }

        public Item GetItem(string path) => ItemTable[path] as Item;


        public void Serialize(string dir) {
            XmlSerializer ser = new XmlSerializer(typeof(CSCache));
            using (FileStream fs = new FileStream(Path.Combine(dir, FILE_NAME), FileMode.Create, FileAccess.Write)) {
                ser.Serialize(fs, this);
            }
        }
        
        public static CSCache Deserialize(string dir) {
            try {
                XmlSerializer ser = new XmlSerializer(typeof(CSCache));
                using (FileStream fs = new FileStream(Path.Combine(dir, FILE_NAME), FileMode.Open, FileAccess.Read)) {
                    var ret = ser.Deserialize(fs) as CSCache;
                    foreach(var item in ret.Items) ret.ItemTable[item.IncludedPath] = item;
                    return ret;
                }
            }
            catch {
                return null;
            }
        }
    }
}
