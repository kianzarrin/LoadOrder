#pragma warning disable 
namespace CO.Packaging {
    using CO.IO;
    using CO.PlatformServices;
    using LoadOrderTool;
    using LoadOrderTool.Util;
    using Mono.Cecil;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.ConstrainedExecution;
    using System.Threading;
    using System.Windows.Forms;
    using LoadOrderShared;

    public class PackageManager : SingletonLite<PackageManager> {
        static ConfigWrapper ConfigWrapper => ConfigWrapper.instance;
        static LoadOrderConfig Config => ConfigWrapper.Config;

        public class AssetInfo {
            private string m_Path;

            //private bool m_IsBuiltin;

            //public bool isBuiltin => m_IsBuiltin;

            public bool IsLocal => publishedFileID == PublishedFileId.invalid;
            public bool IsWorkshop => !IsLocal;

            public string AssetPath => m_Path;

            //public string AssetIncludedPath {
            //    get {
            //        return ContentUtil.ToIncludedPath(m_Path);
            //    }
            //}

            public string AssetName => Path.GetFileNameWithoutExtension(AssetPath);
            public string FileName => Path.GetFileName(AssetPath);

            string displayText_;
            public string DisplayText {
                get {
                    if (string.IsNullOrEmpty(displayText_)) {
                        displayText_ = ConfigAssetInfo.AssetName;
                        if (string.IsNullOrEmpty(displayText_))
                            displayText_ = AssetName;
                    }
                    return displayText_;
                }
            }

            string strTags_;
            public string StrTags => strTags_ ??= 
                ConfigAssetInfo.Tags != null 
                ? string.Join(", ", ConfigAssetInfo.Tags) 
                : "";

            string searchText_;
            public string SearchText => searchText_ ??=
                $"{DisplayText} {publishedFileID} {ConfigAssetInfo.Author}".Trim();

            public PublishedFileId publishedFileID => this.m_PublishedFileID;

            bool isIncludedPending_;
            public bool IsIncludedPending {
                get => isIncludedPending_;
                set {
                    if (isIncludedPending_ != value) {
                        isIncludedPending_ = value;
                        ConfigWrapper.Dirty = true;
                    }
                }
            }

            //public bool IsIncluded {
            //    get => !FileName.StartsWith("_");
            //    set {
            //        isIncludedPending_ = value;
            //        if (value == IsIncluded) return;
            //        Log.Debug($"set_IsIncluded current value = {IsIncluded} | target value = {value}");
            //        string parentPath = Directory.GetParent(m_Path).FullName;
            //        string targetFilename =
            //            value
            //            ? FileName.Substring(1)  // drop _ prefix
            //            : "_" + FileName; // add _ prefix
            //        string targetPath = Path.Combine(parentPath, targetFilename);
            //        MoveFileToPath(targetPath);
            //    }
            //}

            public bool IsIncluded {
                get => !ConfigAssetInfo.Excluded;
                set {
                    isIncludedPending_ = value;
                    ConfigAssetInfo.Excluded = !value;
                }
            }

            //public void MoveFileToPath(string targetPath) {
            //    try {
            //        Log.Debug($"moving asset from {m_Path} to {targetPath}");
            //        File.Move(m_Path, targetPath);
            //        if (File.Exists(targetPath))
            //            Log.Debug($"move successful!");
            //        else {
            //            Log.Debug($"FAILED!");
            //            throw new Exception($"failed to move asset from {m_Path} to {targetPath}");
            //        }
            //        m_Path = targetPath;
            //    } catch (Exception ex) {
            //        Log.Exception(ex);
            //    }
            //}

            private PublishedFileId m_PublishedFileID = PublishedFileId.invalid;

            public IEnumerable<string> GetTags() => 
                ConfigAssetInfo?.Tags ?? new string[] { };

            private AssetInfo() { }

            public AssetInfo(string path, bool builtin, PublishedFileId id) {
                string includedPath = ContentUtil.ToIncludedPathFull(path);
                this.m_Path = includedPath;
                //this.m_IsBuiltin = builtin;
                this.m_PublishedFileID = id;
                this.ConfigAssetInfo = Config.Assets.FirstOrDefault(
                    item => item.Path == includedPath);
                this.ConfigAssetInfo ??= new LoadOrderShared.AssetInfo {
                    Path = includedPath,
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

        private List<AssetInfo> m_Assets = new List<AssetInfo>();

        public IEnumerable<AssetInfo> GetAssets() => m_Assets;

        public string[] GetAllTags() =>
            m_Assets.SelectMany(a => a.GetTags()).Distinct().ToArray();

        public void LoadPackages() {
            try {

                m_Assets = new List<AssetInfo>();

                //this.LoadPackages(Path.Combine(DataLocation.gameContentPath, "Maps"), PublishedFileId.invalid);
                //this.LoadPackages(Path.Combine(DataLocation.gameContentPath, "Scenarios"), PublishedFileId.invalid);
                this.LoadWorkshopPackages();
                this.LoadPackages(DataLocation.stylesPath, PublishedFileId.invalid);
                this.LoadPackages(DataLocation.assetsPath, PublishedFileId.invalid);
                //this.LoadPackages(DataLocation.mapLocation, PublishedFileId.invalid);
                //this.LoadPackages(DataLocation.saveLocation, PublishedFileId.invalid);
                this.LoadPackages(DataLocation.mapThemesPath, PublishedFileId.invalid);
                //this.LoadPackages(DataLocation.scenarioLocation, PublishedFileId.invalid);

                var assets = Config.Assets.Union(
                    m_Assets.Select(item => item.ConfigAssetInfo));
                Config.Assets = assets.ToArray();
                ConfigWrapper.Dirty = true;
            } catch (Exception ex) {
                Log.Exception(ex);
            }
        }

        public void LoadWorkshopPackages() {
            var subscribedItems = ContentUtil.GetSubscribedItems();
            if (subscribedItems != null) {
                foreach (var id in subscribedItems) {
                    string subscribedItemPath = ContentUtil.GetSubscribedItemPath(id);
                    if (subscribedItemPath != null && Directory.Exists(subscribedItemPath)) {
                        //Log.Debug("scanned: " + subscribedItemPath);
                        LoadPackages(subscribedItemPath, id);
                    } else {
                        //Log.Debug("direcotry does not exist: " + subscribedItemPath);
                    }
                }
            }
        }

        public void LoadPackages(string path, PublishedFileId id) {
            CheckFiles(path);
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

        public static void CheckFiles(string path) {
            try {
                foreach (string file in Directory.GetFiles(path))
                    CheckFile(file);
            } catch (Exception ex) {
                Log.Exception(ex);
            }
        }
    
        public static void CheckFile(string fullFilePath) {
            string included = ContentUtil.ToIncludedPath(fullFilePath);
            string excluded = ContentUtil.ToExcludedPath(fullFilePath);
            if (File.Exists(included) && File.Exists(excluded)) {
                File.Move(included, excluded, overwrite:true);
                fullFilePath = excluded;
            }
        }

        public void LoadPackage(string fullFilePath, PublishedFileId id) {
            var package = new PackageManager.AssetInfo(fullFilePath, false, id);
            m_Assets.Add(package);
        }

        public void ApplyPendingValues() {
            foreach (var p in GetAssets())
                p.ApplyPendingValues();
        }

        public AssetInfo GetAssetInfo(string path) =>
            this.GetAssets().FirstOrDefault(p => p.AssetPath == path);

        public void LoadFromProfile(LoadOrderProfile profile, bool excludeExtras = true) {
            foreach (var assetInfo in this.GetAssets()) {
                var assetProfile = profile.GetAsset(assetInfo.AssetPath);
                if (assetProfile != null) {
                    assetProfile.Write(assetInfo);
                } else if (excludeExtras) {
                    //Log.Debug("asset profile with path not found: " + assetInfo.AssetPath);
                    assetInfo.IsIncluded = false;
                }
            }

            var missing = profile.Assets.Where(m => GetAssetInfo(m.IncludedPath) == null);
            if (missing.Any()) {
                var strMissing = string.Join('\n', missing.Select(p => p.DisplayText).ToArray());
                MessageBox.Show(strMissing, "Warning! Missing Assets", MessageBoxButtons.OK);
            }
        }

        public void SaveToProfile(LoadOrderProfile profile) {
            var list = new List<LoadOrderProfile.Asset>(m_Assets.Count);
            foreach (var assetInfo in m_Assets) {
                var assetProfile = new LoadOrderProfile.Asset(assetInfo);
                list.Add(assetProfile);
            }
            profile.Assets = list.ToArray();
        }
    }
}
