namespace LoadOrderShared {
    using System.IO;
    using System.Xml.Serialization;
    using System.Collections.Generic;
    using System.Collections;
    using System.Linq;

    public class CSCache {
        public class Item {
            public string IncludedPath;
            public string Name;
            public string Description;
        }

        public class Mod: Item { }

        public class Asset: Item {
            public string[] Tags;
        }

        public const string FILE_NAME = "CSCache.xml";

        public string WorkShopContentPath;
        public string GamePath;
        public string SteamPath;


        // do not use these directly. use Add/GetItem insteaed.
        public Mod[] Mods = new Mod[0];
        public Asset[] Assets = new Asset[0];

        internal Hashtable ItemTable = new Hashtable(100000);

        public void AddItem(Item item) {
            ItemTable[item.IncludedPath] = item;
        }

        public Item GetItem(string path) => ItemTable[path] as Item;


        public void Serialize(string dir) {
            Mods = ItemTable.Values.OfType<Mod>().ToArray();
            Assets = ItemTable.Values.OfType<Asset>().ToArray();
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
                    foreach (var item in ret.Mods) ret.ItemTable[item.IncludedPath] = item;
                    foreach (var item in ret.Assets) ret.ItemTable[item.IncludedPath] = item;
                    return ret;
                }
            }
            catch {
                return null;
            }
        }
    }
}
