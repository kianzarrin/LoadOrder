namespace LoadOrderMod.Settings {
    extern alias Injections;
    using Injections.LoadOrderInjections;
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
    using System.Threading;
    using static ColossalFramework.Plugins.PluginManager;
    using static KianCommons.ReflectionHelpers;
    using System.Globalization;
    using System.Collections;

    public static class ConfigUtil {
        private static Hashtable assetsTable_;

        private static LoadOrderConfig config_;
        public static LoadOrderConfig Config {
            get {
                try {
                    Init();
                    return config_;
                } catch (Exception ex) {
                    Log.Exception(ex);
                    return null;
                }
            }
        }

        private static void Init() {
            if (config_ != null) return; // already initialized.
            LogCalled();
            config_ =
                LoadOrderConfig.Deserialize(DataLocation.localApplicationData)
                ?? new LoadOrderConfig();

            int n = Math.Max(PlatformService.workshop.GetSubscribedItemCount(),  config_.Assets.Length);
            assetsTable_ = new Hashtable(n * 10);
            foreach(var assetInfo in config_.Assets)
                assetsTable_[assetInfo.Path] = assetInfo;
                
            SaveThread.Init();
        }

        public static void Terminate() {
            LogCalled();
            SaveThread.Terminate();
            config_ = null;
        }

        public static void SaveConfig() {
            try {
                //LogCalled();
                SaveThread.Dirty = false;
                if (config_ == null) return;
                lock (SaveThread.LockObject)
                    config_.Serialize(DataLocation.localApplicationData);
            } catch (Exception ex) {
                Log.Exception(ex);
            }
        }


        // this is useful to store author details after call back.
        internal static class SaveThread {
            const int INTERVAL_MS = 1000;
            static bool isRunning_;
            private static Thread thread_;

            public static bool Dirty = false;
            public readonly static object LockObject = new object();

            static SaveThread() => Init();

            internal static void Init() {
                if (isRunning_ != null) return; // already running.
                thread_ = new Thread(RunThread);
                thread_.Name = "SaveThread";
                thread_.IsBackground = true;
                isRunning_ = true;
                thread_.Start();
            }

            internal static void Terminate() {
                Flush();
                isRunning_ = false;
                thread_ = null;
            }

            private static void RunThread() {
                try {
                    while (isRunning_) {
                        Thread.Sleep(INTERVAL_MS);
                        Flush();
                    }
                    Log.Info("Save Thread Exiting...");
                } catch (Exception ex) {
                    Log.Exception(ex);
                }
            }
            public static void Flush() {
                if (Dirty)
                    SaveConfig();
            }
        }



        public static void AquirePathDetails() {
            try {
                LogCalled();
                Config.GamePath = DataLocation.applicationBase;
                Log.Info("Config.GamePath=" + Config.GamePath,true);
                foreach (var pluginInfo in PluginManager.instance.GetPluginsInfo()) {
                    if (pluginInfo.publishedFileID != PublishedFileId.invalid) {
                        Config.WorkShopContentPath = Path.GetDirectoryName(pluginInfo.modPath);
                        Log.Info("Config.WorkShopContentPath=" + Config.WorkShopContentPath,true);
                        break;
                    }
                }
                
            } catch (Exception ex) {
                Log.Exception(ex);
            }
        }

        public static void AquireModsDetails() {
            LogCalled();
            foreach (var pluginInfo in PluginManager.instance.GetPluginsInfo()) {
                try {
                    if (pluginInfo?.userModInstance == null) continue;
                    var modInfo = pluginInfo.GetModConfig();
                    if (modInfo == null) {
                        modInfo = new LoadOrderShared.ModInfo {
                            Path = pluginInfo.modPath,
                            LoadOrder = LoadOrderConfig.DefaultLoadOrder,
                        };
                        Config.Mods = Config.Mods.AddToArray(modInfo);
                    }
                    Assertion.NotNull(modInfo);

                    modInfo.Description = pluginInfo.GetUserModInstance()?.Description;
                    modInfo.Name = pluginInfo.GetModName();
                    string author = pluginInfo.GetAuthor();
                    if (author.IsAuthorNameValid())
                        modInfo.Author = author;
                    var entry = pluginInfo.GetEntryData();
                    if (entry != null && entry.updated != default)
                        modInfo.DateUpdated = entry.updated.ToLocalTime().ToString(CultureInfo.InvariantCulture);

                    if (pluginInfo.publishedFileID != PublishedFileId.invalid &&
                        entry != null &&
                        entry.workshopDetails.publishedFileId == pluginInfo.publishedFileID) {
                        modInfo.Status = (DownloadStatus)(int)
                            SteamUtilities.IsUGCUpToDate(entry.workshopDetails, out modInfo.DownloadFailureReason);
                    }
                } catch (Exception ex) {
                    ex.Log("pluginInfo=" + pluginInfo);
                }
            }
        }

        public static void AquireAssetsDetails() {
            LogCalled();

            foreach (var asset in PackageManager.FilterAssets(new[] {
                UserAssetType.CustomAssetMetaData,
                UserAssetType.MapThemeMetaData,
                UserAssetType.ColorCorrection,
                UserAssetType.DistrictStyleMetaData,
            })) {
                try {
                    Assertion.NotNull(asset, "asset");
                    if (!asset.isMainAsset) continue;
                    var assetInfo = asset.GetAssetConfig();
                    if (assetInfo == null) {
                        assetInfo = new AssetInfo { Path = asset.GetPath() };
                        AddAssetConfig(assetInfo);
                    }
                    Assertion.NotNull(assetInfo, "assetInfo");
                    Assertion.NotNull(asset.package, "asset.package");

                    assetInfo.AssetName = asset.name;

                    // get asset name from file (which could be less complete)
                    // if we don't already have a complete name
                    bool fallback = !assetInfo.Author.IsAuthorNameValid();
                    {
                        string author = asset.GetAuthor(fallback);
                        if (author.IsAuthorNameValid())
                            assetInfo.Author = author;
                    }

                    var entry = asset.GetEntryData();

                    MetaData metaData = asset.Instantiate() as MetaData;
                    if (entry != null && entry.updated != default)
                        assetInfo.DateUpdated = entry.updated.ToLocalTime().ToString(CultureInfo.InvariantCulture);
                    else
                        assetInfo.DateUpdated = metaData.getTimeStamp.ToLocalTime().ToString(CultureInfo.InvariantCulture);

                    assetInfo.Tags = asset.type.Tags();
                    Assertion.NotNull(assetInfo.Tags, "assetInfo.Tags");

                    if (metaData is CustomAssetMetaData customAssetMetaData) {
                        assetInfo.description = ContentManagerUtil.SafeGetAssetDesc(customAssetMetaData, asset.package);
                        var tags = customAssetMetaData.Tags(asset.package.GetPublishedFileID());
                        assetInfo.Tags = assetInfo.Tags.Concat(tags).ToArray();
                    }

                    if (asset.package.GetPublishedFileID() != PublishedFileId.invalid &&
                        entry != null &&
                        entry.workshopDetails.publishedFileId == asset.package.GetPublishedFileID()) {
                        assetInfo.Status = (DownloadStatus)(int)
                            SteamUtilities.IsUGCUpToDate(entry.workshopDetails, out assetInfo.DownloadFailureReason);
                    }
                } catch(Exception ex) {
                    ex.Log($"asset: {asset}");
                }
            }
        }

        public static void StoreConfigDetails() {
            LogCalled();
            try {
                AquirePathDetails();
                AquireModsDetails();
                AquireAssetsDetails();
                SaveConfig();
            } catch (Exception ex) {
                Log.Exception(ex);
            }
        }

        internal static bool HasLoadOrder(this PluginInfo p) {
            var mod = p.GetModConfig();
            if (mod == null)
                return false;
            return mod.LoadOrder != LoadOrderConfig.DefaultLoadOrder;
        }

        internal static int GetLoadOrder(this PluginInfo p) {
            var mod = p.GetModConfig();
            if (mod == null)
                return LoadOrderConfig.DefaultLoadOrder;
            return mod.LoadOrder;
        }

        internal static LoadOrderShared.ModInfo GetModConfig(this PluginInfo p) =>
            Config?.Mods?.FirstOrDefault(item => item.Path == p.modPath);

        internal static void AddAssetConfig(AssetInfo assetInfo) {
            Config.Assets = Config.Assets.AddToArray(assetInfo);
            assetsTable_[assetInfo.Path] = assetInfo;
        }

        internal static LoadOrderShared.AssetInfo GetAssetConfig(this Package.Asset a) =>
            assetsTable_[a.GetPath()] as AssetInfo;
        
        internal static string GetPath(this Package.Asset a) => a.package.packagePath;

        internal static void SetAuthor(this Package.Asset a, string author) {
            if (a.GetAssetConfig() is AssetInfo assetInfo) {
                assetInfo.Author = author;
                SaveThread.Dirty = true;
            }
        }

        internal static void SetAuthor(this PluginInfo p, string author) {
            if (p.GetModConfig() is ModInfo modInfo) {
                modInfo.Author = author;
                SaveThread.Dirty = true;
            }
        }
        internal static void SetDate(this Package.Asset a, DateTime date) {
            if (a.GetAssetConfig() is AssetInfo assetInfo) {
                assetInfo.DateUpdated = date.ToLocalTime().ToString(CultureInfo.InvariantCulture);
                SaveThread.Dirty = true;
            }
        }

        internal static void SetDate(this PluginInfo p, DateTime date) {
            if (p.GetModConfig() is ModInfo modInfo) {
                modInfo.DateUpdated = date.ToLocalTime().ToString(CultureInfo.InvariantCulture);
                SaveThread.Dirty = true;
            }
        }
    }
}
