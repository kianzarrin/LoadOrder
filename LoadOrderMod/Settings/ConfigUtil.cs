namespace LoadOrderMod.Settings {
    using ColossalFramework.Globalization;
    using ColossalFramework.IO;
    using ColossalFramework.Packaging;
    using ColossalFramework.PlatformServices;
    using ColossalFramework.Plugins;
    using HarmonyLib;
    using KianCommons;
    using KianCommons.Plugins;
    using LoadOrderMod.Util;
    using LoadOrderShared;
    using System;
    using System.IO;
    using System.Linq;
    using static ColossalFramework.Plugins.PluginManager;
    using static KianCommons.ReflectionHelpers;

    public static class ConfigUtil {
        internal static LoadOrderConfig config_;
        public static LoadOrderConfig Config {
            get {
                try {
                    return config_ ??=
                        LoadOrderConfig.Deserialize(DataLocation.localApplicationData)
                        ?? new LoadOrderConfig();
                } catch(Exception ex) {
                    Log.Exception(ex);
                    return null;
                }
            }
        }

        public static void SaveConfig() =>
            config_?.Serialize(DataLocation.localApplicationData);

        public static void AquirePathDetails() {
            try {
                Config.GamePath = DataLocation.applicationBase;
                foreach(var pluginInfo in PluginManager.instance.GetPluginsInfo()) {
                    if(pluginInfo.publishedFileID != PublishedFileId.invalid) {
                        Config.WorkShopContentPath = Path.GetDirectoryName(pluginInfo.modPath);
                        break;
                    }
                }
            } catch(Exception ex) {
                Log.Exception(ex);
            }
        }

        public static void AquireModsDetails() {
            foreach(var pluginInfo in PluginManager.instance.GetPluginsInfo()) {
                try {
                    var modInfo = pluginInfo.GetModConfig();
                    if(modInfo == null) {
                        modInfo = new LoadOrderShared.ModInfo {
                            Path = pluginInfo.modPath,
                            LoadOrder = LoadOrderConfig.DefaultLoadOrder,
                        };
                        Config.Mods = Config.Mods.AddToArray(modInfo);
                    }
                    modInfo.Description = pluginInfo.GetUserModInstance().Description;
                    modInfo.ModName = pluginInfo.GetModName();
                } catch(Exception ex) {
                    Log.Exception(ex);
                }
            }
        }

        public static void AquireAssetsDetails() {
            LogCalled();
            foreach(var asset in PackageManager.FilterAssets(UserAssetType.CustomAssetMetaData)) {
                try {
                    if(!asset.isMainAsset) continue;
                    string path = asset.package.packagePath;
                    var assetInfo = asset.GetAssetConfig();
                    if(assetInfo == null) {
                        assetInfo = new LoadOrderShared.AssetInfo { Path = asset.GetPath() };
                        Config.Assets = Config.Assets.AddToArray(assetInfo);
                    }
                    CustomAssetMetaData metaData = asset.Instantiate<CustomAssetMetaData>();
                    assetInfo.AssetName = asset.name;
                    assetInfo.Author = asset.GetAuthor();
                    assetInfo.description = SafeGetAssetDesc(metaData, asset.package);
                    assetInfo.Date = metaData.getTimeStamp.ToLocalTime().ToString();
                    assetInfo.Tags = metaData.Tags();
                } catch(Exception ex) {
                    Log.Exception(ex);
                }
            }
        }

        public static void StoreConfigDetails() {
            try {
                AquirePathDetails();
                AquireModsDetails();
                AquireAssetsDetails();
                SaveConfig();
            } catch(Exception ex) {
                Log.Exception(ex);
            }
        }

        internal static bool HasLoadOrder(this PluginInfo p) {
            var mod = p.GetModConfig();
            if(mod == null)
                return false;
            return mod.LoadOrder != LoadOrderConfig.DefaultLoadOrder;
        }

        internal static int GetLoadOrder(this PluginInfo p) {
            var mod = p.GetModConfig();
            if(mod == null)
                return LoadOrderConfig.DefaultLoadOrder;
            return mod.LoadOrder;
        }

        internal static LoadOrderShared.ModInfo GetModConfig(this PluginInfo p) =>
            Config?.Mods?.FirstOrDefault(item => item.Path == p.modPath);

        internal static LoadOrderShared.AssetInfo GetAssetConfig(this Package.Asset a) =>
            Config?.Assets?.FirstOrDefault(item => item.Path == a.GetPath());
        internal static string GetPath(this Package.Asset a) => a.package.packagePath;

        static string GetAuthor(this Package.Asset a) => ResolveName(a.package.packageAuthor);
        static string ResolveName(string str) {
            string[] array = str.Split(':');
            if(array.Length == 2 &&
                array[0] == "steamid" &&
                ulong.TryParse(array[1], out ulong value) &&
                value > 0) {
                return new Friend(new UserID(value)).personaName;
            }
            return null;
        }

        private static string SafeGetAssetDesc(CustomAssetMetaData metaData, Package package) {
            string localeID;
            if(metaData.type == CustomAssetMetaData.Type.Building) {
                localeID = "BUILDING_DESC";
            } else if(metaData.type == CustomAssetMetaData.Type.Prop) {
                localeID = "PROPS_DESC";
            } else if(metaData.type == CustomAssetMetaData.Type.Tree) {
                localeID = "TREE_DESC";
            } else {
                if(metaData.type != CustomAssetMetaData.Type.Road) {
                    return metaData.name;
                }
                localeID = "NET_DESC";
            }
            return SafeGetAssetString(localeID, metaData.name, package);
        }

        private static string SafeGetAssetString(string localeID, string assetName, Package package) {
            if(package != null) {
                string key = package.packageName + "." + assetName + "_Data";
                if(Locale.Exists(localeID, key)) {
                    return Locale.Get(localeID, key);
                }
            }
            return assetName;
        }
    }
}
