namespace LoadOrderMod.Util {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ColossalFramework.Packaging;
    using ColossalFramework.Plugins;
    using ColossalFramework.PlatformServices;

    public static class TagsUtil {
        internal static Dictionary<string, string> Tag2Category;
        static TagsUtil() {
            Tag2Category = new Dictionary<string, string>() {
                {SteamHelper.kSteamTagRoad, "Network"},
            };
        }

        public static string[] Tags(this CustomAssetMetaData metadata, PublishedFileId publishedFileId) {
            var ret = new List<string>();
            foreach(var tag in metadata.steamTags) {
                if(Tag2Category.TryGetValue(tag, out var cat))
                    ret.Add(cat);
                else ret.Add(tag);
            }

            if (publishedFileId != PublishedFileId.invalid &&
                ContentManagerUtil.ModEntries.Any(item => item.publishedFileId == publishedFileId)) {
                ret.Add("Mod");
            }

            return ret.ToArray();
        }

    }
}
