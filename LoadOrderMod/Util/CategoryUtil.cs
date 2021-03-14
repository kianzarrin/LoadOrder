namespace LoadOrderMod.Util {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ColossalFramework.Packaging;

    public static class CategoryUtil {
        internal static Dictionary<string, string> Tag2Category;
        static CategoryUtil() {
            Tag2Category = new Dictionary<string, string>() {
                {SteamHelper.kSteamTagRoad, "Network"},
            };
        }

        public static string[] Tags(this CustomAssetMetaData metadata) {
            var ret = new List<string>();
            foreach(var tag in metadata.steamTags) {
                if(Tag2Category.TryGetValue(tag, out var cat))
                    ret.Add(cat);
                else ret.Add(tag);
            }
            return ret.ToArray();
        }

    }
}
