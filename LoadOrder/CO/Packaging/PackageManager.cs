#pragma warning disable 
namespace CO.Packaging {
    using CO.IO;
    using CO.PlatformServices;
    using LoadOrderTool;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.ConstrainedExecution;
    using LoadOrderTool.Util;
    using Mono.Cecil;
    using System.Threading;

    public class PackageManager : SingletonLite<PackageManager> {
        public ConfigWrapper ConfigWrapper => Plugins.PluginManager.instance.ConfigWrapper;
        LoadOrderShared.LoadOrderConfig Config => ConfigWrapper.Config;

        public class AssetInfo {
            // private Type m_UserModImplementation;
            // public List<Assembly> m_Assemblies;
            // public int assemblyCount => m_Assemblies.Count;

            private string m_Path;

            private bool m_IsBuiltin;

            public bool isBuiltin => m_IsBuiltin;

            public string AssetPath => m_Path;


            public string AssetIncludedPath {
                get {
                    if (FileName.StartsWith("_"))
                        return Path.Combine(parentDirPath, FileName.Substring(1));
                    else
                        return AssetPath;
                }
            }

            public string AssetName => Path.GetFileNameWithoutExtension(AssetPath);
            public string FileName => Path.GetFileName(AssetPath);
            public string parentDirPath => Path.GetDirectoryName(AssetPath);



            public string DisplayText {
                get {
                    string ret = ConfigAssetInfo.AssetName;
                    if(string.IsNullOrEmpty(ret))
                        ret = AssetName;
                    if (publishedFileID != PublishedFileId.invalid)
                        ret = $"{publishedFileID.AsUInt64}: " + ret;
                    return ret;
                }
            }


            public PublishedFileId publishedFileID => this.m_PublishedFileID;

            bool isIncludedPending_;
            public bool IsIncludedPending {
                get => isIncludedPending_;
                set {
                    isIncludedPending_ = value;
                    PackageManager.instance.ConfigWrapper.Dirty = true;
                }
            }

            public bool IsIncluded {
                get => !FileName.StartsWith("_");
                set {
                    Log.Debug($"set_IsIncluded current value = {IsIncluded} | target value = {value}");
                    IsIncludedPending = value;
                    if (value == IsIncluded)
                        return;
                    string parentPath = Directory.GetParent(m_Path).FullName;
                    string targetFilename =
                        value
                        ? FileName.Substring(1)  // drop _ prefix
                        : "_" + FileName; // add _ prefix
                    string targetPath = Path.Combine(parentPath, targetFilename);
                    MoveToPath(targetPath);
                }
            }

            public void MoveToPath(string targetPath) {
                try {
                    Log.Debug($"moving mod from {AssetPath} to {targetPath}");
                    Directory.Move(AssetPath, targetPath);
                    if (Directory.Exists(targetPath))
                        Log.Debug($"move successful!");
                    else {
                        Log.Debug($"FAILED!");
                        throw new Exception("failed to move directory from {ModPath} to {targetPath}");
                    }
                    m_Path = targetPath;
                } catch (Exception ex) {
                    Log.Exception(ex);
                }
            }

            private PublishedFileId m_PublishedFileID = PublishedFileId.invalid;

            private AssetInfo() {
            }

            public AssetInfo(string path, bool builtin, PublishedFileId id) {
                this.m_Path = path;
                this.m_IsBuiltin = builtin;
                this.m_PublishedFileID = id;
                this.ConfigAssetInfo = PackageManager.instance.Config.Assets.FirstOrDefault(
                    item => item.Path == AssetIncludedPath);
                this.ConfigAssetInfo ??= new LoadOrderShared.AssetInfo {
                    Path = AssetIncludedPath,
                };
                isIncludedPending_ = IsIncluded;
            }

                   public LoadOrderShared.AssetInfo ConfigAssetInfo { get; private set; }

            public override string ToString() {
                return 
                    $"AssetInfo: path={AssetPath} " +
                    $"included={IsIncludedPending} " +
                    $"DisplayText={DisplayText} " +
                    $"FileName={FileName}";
            }

            public void ApplyPendingValues() {
                IsIncluded = isIncludedPending_;
            }
        }

        public static readonly string packageExtension = ".crp";

        //private Dictionary<string, AssetInfo> m_Plugins = new Dictionary<string, AssetInfo>();
        private List<AssetInfo> m_Assets = new List<AssetInfo>();

        public IEnumerable<AssetInfo> GetAssets() => m_Assets;

        
        public void LoadPackages() {
            try {

                m_Assets = new List<AssetInfo>();

                this.LoadPackages(Path.Combine(DataLocation.gameContentPath, "Maps") , PublishedFileId.invalid);
                this.LoadPackages(Path.Combine(DataLocation.gameContentPath, "Scenarios"), PublishedFileId.invalid);
                this.LoadWorkshopPackages();
                this.LoadPackages(DataLocation.stylesPath, PublishedFileId.invalid);
                this.LoadPackages(DataLocation.assetsPath, PublishedFileId.invalid);
                this.LoadPackages(DataLocation.mapLocation, PublishedFileId.invalid);
                this.LoadPackages(DataLocation.saveLocation, PublishedFileId.invalid);
                this.LoadPackages(DataLocation.mapThemesPath, PublishedFileId.invalid);
                this.LoadPackages(DataLocation.scenarioLocation, PublishedFileId.invalid);

                var assets = Config.Assets.Union(
                    m_Assets.Select(item => item.ConfigAssetInfo));
                Config.Assets = assets.ToArray();
                ConfigWrapper.Dirty = true;
            }catch (Exception ex) {
                Log.Exception(ex);
            }
        }

        public void LoadWorkshopPackages() {
            var subscribedItems = ContentUtil.GetSubscribedItems();
            if (subscribedItems != null) {
                foreach (var id in subscribedItems) {
                    string subscribedItemPath = ContentUtil.GetSubscribedItemPath(id);
                    if (subscribedItemPath != null && Directory.Exists(subscribedItemPath)) {
                        Log.Debug("scanned: " + subscribedItemPath);
                        LoadPackages(subscribedItemPath, id);
                    } else {
                        Log.Debug("direcotry does not exist: " + subscribedItemPath);
                    }
                }
            }
        }

        public void LoadPackages(string path, PublishedFileId id) {
            try {
                foreach (string file in Directory.GetFiles(path)) {
                    try {
                        if (Path.GetExtension(file) == PackageManager.packageExtension) {
                            LoadPackage(file, id);
                        }
                    } catch (Exception ex) {
                        Log.Exception(ex);
                    }
                }
                foreach (string path2 in Directory.GetDirectories(path)) {
                    this.LoadPackages(path2, id);
                }
            } catch (Exception ex) {
                Log.Exception(ex);
            }
        }

        public void LoadPackage(string fileName, PublishedFileId id) {
            var package = new PackageManager.AssetInfo(fileName, false, id);
            m_Assets.Add(package);
        }


        public void ApplyPendingValues() {
            foreach(var p in GetAssets())
                p.ApplyPendingValues();
        }
    }
}
