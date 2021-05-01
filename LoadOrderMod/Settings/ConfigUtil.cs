namespace LoadOrderMod.Settings {
    using ColossalFramework;
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
    using System.Threading;
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
                } catch (Exception ex) {
                    Log.Exception(ex);
                    return null;
                }
            }
        }

        public static void SaveConfig() {
            LogCalled();
            if (config_ == null) return;
            lock (SaveThread.LockObject) {
                SaveThread.Dirty = false;
                config_.Serialize(DataLocation.localApplicationData);
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
                thread_ = new Thread(RunThread);
                thread_.Name = "SaveThread";
                thread_.IsBackground = true;
                isRunning_ = true;
                thread_.Start();
            }

            internal static void Terminate() {
                isRunning_ = false;
                LogCalled();
            }

            private static void RunThread() {
                try {
                    while (isRunning_) {
                        Thread.Sleep(INTERVAL_MS);
                        Flush();
                    }
                    Flush();
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
                Config.GamePath = DataLocation.applicationBase;
                foreach (var pluginInfo in PluginManager.instance.GetPluginsInfo()) {
                    if (pluginInfo.publishedFileID != PublishedFileId.invalid) {
                        Config.WorkShopContentPath = Path.GetDirectoryName(pluginInfo.modPath);
                        break;
                    }
                }
            } catch (Exception ex) {
                Log.Exception(ex);
            }
        }

        public static void AquireModsDetails() {
            foreach (var pluginInfo in PluginManager.instance.GetPluginsInfo()) {
                try {
                    if (pluginInfo.userModInstance == null) continue;
                    var modInfo = pluginInfo.GetModConfig();
                    if (modInfo == null) {
                        modInfo = new LoadOrderShared.ModInfo {
                            Path = pluginInfo.modPath,
                            LoadOrder = LoadOrderConfig.DefaultLoadOrder,
                        };
                        Config.Mods = Config.Mods.AddToArray(modInfo);
                    }
                    modInfo.Description = pluginInfo.GetUserModInstance()?.Description;
                    modInfo.ModName = pluginInfo.GetModName();
                    string author = pluginInfo.GetAuthor();
                    if (author.IsAuthorNameValid())
                        modInfo.Author = author;
                    // TODO: listen to events to get name.
                } catch (Exception ex) {
                    Log.Exception(ex);
                }
            }
        }

        public static void AquireAssetsDetails() {
            foreach (var asset in PackageManager.FilterAssets(UserAssetType.CustomAssetMetaData)) {
                try {
                    if (!asset.isMainAsset) continue;
                    string path = asset.package.packagePath;
                    var assetInfo = asset.GetAssetConfig();
                    if (assetInfo == null) {
                        assetInfo = new LoadOrderShared.AssetInfo { Path = asset.GetPath() };
                        Config.Assets = Config.Assets.AddToArray(assetInfo);
                    }
                    CustomAssetMetaData metaData = asset.Instantiate<CustomAssetMetaData>();
                    assetInfo.AssetName = asset.name;

                    // get asset name from file (which could be less complete)
                    // if we don't already have a complete name
                    bool fallback = !assetInfo.Author.IsAuthorNameValid();
                    string author = asset.GetAuthor(fallback);
                    if (author.IsAuthorNameValid())
                        assetInfo.Author = author;

                    assetInfo.description = ContentManagerUtil.SafeGetAssetDesc(metaData, asset.package);
                    assetInfo.Date = metaData.getTimeStamp.ToLocalTime().ToString();
                    assetInfo.Tags = metaData.Tags(asset.package.GetPublishedFileID());
                } catch (Exception ex) {
                    Log.Exception(ex);
                }
            }
        }

        public static void StoreConfigDetails() {
            try {
                //PlatformService.eventPersonaStateChange += OnNameReceived;
                //PlatformService.workshop.eventUGCRequestUGCDetailsCompleted += OnDetailsReceived;
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

        internal static LoadOrderShared.AssetInfo GetAssetConfig(this Package.Asset a) =>
            Config?.Assets?.FirstOrDefault(item => item.Path == a.GetPath());
        internal static string GetPath(this Package.Asset a) => a.package.packagePath;

        internal static void SetAuthor(this Package.Asset a, string author) {
            if(a.GetAssetConfig() is AssetInfo assetInfo) {
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
    }
}
