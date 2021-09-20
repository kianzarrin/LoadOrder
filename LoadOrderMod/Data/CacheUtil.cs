namespace LoadOrderMod.Data {
    using ColossalFramework.IO;
    using ColossalFramework.Packaging;
    using ColossalFramework.PlatformServices;
    using ColossalFramework.Plugins;
    using KianCommons;
    using KianCommons.Plugins;
    using LoadOrderMod.Settings;
    using LoadOrderMod.Util;
    using LoadOrderShared;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using static ColossalFramework.Plugins.PluginManager;
    using static KianCommons.ReflectionHelpers;

    public class CacheUtil {
        public CSCache Cache;
        public CacheUtil() {
            Load();
        }
        public void Load() => Cache = CSCache.Deserialize(ConfigUtil.LocalLoadOrderPath) ?? new CSCache();
        public void Save() => Cache.Serialize(ConfigUtil.LocalLoadOrderPath);

        internal CSCache.Mod GetOrCreateModCache(PluginInfo p) {
            var ret = Cache.GetItem(p.modPath) as CSCache.Mod;
            if (ret == null) {
                ret = new CSCache.Mod { IncludedPath = p.modPath };
                Cache.AddItem(ret);
            }
            return ret;
        }

        internal CSCache.Asset GetOrCreateAssetCache(Package.Asset a) {
            var ret = Cache.GetItem(a.GetPath()) as CSCache.Asset;
            if (ret == null) {
                ret = new CSCache.Asset { IncludedPath = a.GetPath() };
                Cache.AddItem(ret);
            }
            return ret;
        }

        public void AquirePathDetails() {
            try {
                LogCalled();
                Cache.GamePath = DataLocation.applicationBase;
                Log.Info("Config.GamePath=" + Cache.GamePath, true);
                foreach (var pluginInfo in PluginManager.instance.GetPluginsInfo()) {
                    if (pluginInfo.publishedFileID != PublishedFileId.invalid) {
                        Cache.WorkShopContentPath = Path.GetDirectoryName(pluginInfo.modPath);
                        Log.Info("Config.WorkShopContentPath=" + Cache.WorkShopContentPath, true);
                        break;
                    }
                }

            } catch (Exception ex) {
                Log.Exception(ex);
            }
        }

        public void AquireModsDetails() {
            LogCalled();
            foreach (var pluginInfo in PluginManager.instance.GetPluginsInfo()) {
                try {
                    if (pluginInfo?.userModInstance == null) continue;
                    var cache = GetOrCreateModCache(pluginInfo);
                    Assertion.NotNull(cache);

                    cache.Description = pluginInfo.GetUserModInstance()?.Description;
                    cache.Name = pluginInfo.GetModName();
                } catch (Exception ex) {
                    ex.Log("pluginInfo=" + pluginInfo);
                }
            }
            LogSucceeded();
        }

        public void AquireAssetsDetails() {
            LogCalled();
            var timerInstantiate = new Stopwatch();
            int assetCount = 0;

            foreach (var asset in PackageManager.FilterAssets(new[] {
                UserAssetType.CustomAssetMetaData,
                UserAssetType.MapThemeMetaData,
                UserAssetType.ColorCorrection,
                UserAssetType.DistrictStyleMetaData,
            })) {
                try {
                    Assertion.NotNull(asset, "asset");
                    if (!asset.isMainAsset) continue;
                    if (asset.GetPath().IsNullorEmpty())
                        continue; // TODO support LUT .png

                    var cache = GetOrCreateAssetCache(asset);
                    Assertion.NotNull(cache, "assetInfo");
                    Assertion.NotNull(asset.package, "asset.package");

                    cache.Name = asset.name;


                    cache.Tags = asset.type.Tags();
                    Assertion.NotNull(cache.Tags, "assetInfo.Tags");

                    timerInstantiate.Start();
                    MetaData metaData = asset.Instantiate() as MetaData;
                    timerInstantiate.Stop();
                    assetCount++;

                    if (metaData is CustomAssetMetaData customAssetMetaData) {
                        cache.Description = ContentManagerUtil.SafeGetAssetDesc(customAssetMetaData, asset.package);
                        var tags = customAssetMetaData.Tags(asset.package.GetPublishedFileID());
                        cache.Tags = cache.Tags.Concat(tags).ToArray();
                    }
                } catch (Exception ex) {
                    ex.Log($"asset: {asset}");
                }
            }

            double ms = timerInstantiate.ElapsedMilliseconds;
            Log.Debug("average asset instantiation time = " + ms / assetCount);

            LogSucceeded();
        }

        public static void CacheData() {
            new CacheUtil().CacheAll();
        }

        public void CacheAll() {
            LogCalled();
            if (!ConfigUtil.Config.UGCCache) {
                Log.Info("Skipping CacheAll ...");
                return;
            }

            try {
                AquirePathDetails();
                Save();
                AquireModsDetails();
                Save();
                AquireAssetsDetails();
                Save();
            } catch (Exception ex) { Log.Exception(ex); }
        }
    }
}
