namespace LoadOrderTool.Data {
    using CO.IO;
    using System.IO;
    using System;
    using System.Linq;
    using System.Xml.Serialization;
    using LoadOrderTool.Util;

    public interface IProfileItem {
        string GetIncludedPath();
        bool TryGetID(out ulong id);
        string GetDisplayText();
        string GetCategoryName(); // mod or asset

    }

    public class LoadOrderProfile {

        const string LOCAL_APP_DATA_PATH = "%LOCALAPPDATA%";
        const string CITIES_PATH = "%CITIES%";
        const string WS_CONTENT_PATH = "%WORKSHOP%";

        static string FromFinalPath(string path) {
            return path
                .Replace(DataLocation.localApplicationData, LOCAL_APP_DATA_PATH)
                .Replace(DataLocation.GamePath, CITIES_PATH)
                .Replace(DataLocation.WorkshopContentPath, WS_CONTENT_PATH);
        }

        static string ToFinalPath(string path) {
            return path
                .Replace(LOCAL_APP_DATA_PATH, DataLocation.localApplicationData)
                .Replace(CITIES_PATH, DataLocation.GamePath)
                .Replace(WS_CONTENT_PATH, DataLocation.WorkshopContentPath);
        }

        public class Mod : IProfileItem {

            [XmlIgnore]
            public string IncludedPathFinal;
            public string IncludedPath {
                get => FromFinalPath(IncludedPathFinal);
                set => IncludedPathFinal = ToFinalPath(value);
            }

            public bool IsEnabled;
            public bool IsIncluded;
            public int LoadOrder;
            public string DisplayText;

            public Mod() { }

            public Mod(CO.Plugins.PluginManager.PluginInfo pluginInfo) {
                IncludedPathFinal = pluginInfo.IncludedPath;
                IsIncluded = pluginInfo.IsIncludedPending;
                IsEnabled = pluginInfo.IsEnabledPending;
                LoadOrder = pluginInfo.LoadOrder;
                DisplayText = pluginInfo.DisplayText;
            }

            public void WriteTo(CO.Plugins.PluginManager.PluginInfo pluginInfo) {
                pluginInfo.IsIncludedPending = IsIncluded;
                pluginInfo.IsEnabledPending = IsEnabled;
                pluginInfo.LoadOrder = LoadOrder;
            }

            public string GetIncludedPath() => IncludedPathFinal;

            public bool TryGetID(out ulong id) => ContentUtil.TryGetModID(IncludedPathFinal, out id);

            public string GetDisplayText() => DisplayText;

            public string GetCategoryName() => "Mod";
        }

        public class Asset : IProfileItem {
            [XmlIgnore]
            public string IncludedPathFinal;

            /// <summary>
            /// only for storage. use the final path instead
            /// </summary>
            public string IncludedPath {
                get => FromFinalPath(IncludedPathFinal);
                set => IncludedPathFinal = ToFinalPath(value);
            }

            public bool IsIncluded;
            public string DisplayText;

            public Asset() { }

            public Asset(CO.Packaging.PackageManager.AssetInfo assetInfo) {
                IncludedPathFinal = assetInfo.AssetPath;
                IsIncluded = assetInfo.IsIncludedPending;
                DisplayText = assetInfo.DisplayText;
            }

            public void WriteTo(CO.Packaging.PackageManager.AssetInfo assetInfo) {
                assetInfo.IsIncludedPending = IsIncluded;
            }
            public string GetIncludedPath() => IncludedPathFinal;

            public bool TryGetID(out ulong id) => ContentUtil.TryGetAssetID(IncludedPathFinal, out id);

            public string GetDisplayText() => DisplayText;

            public string GetCategoryName() => "Asset";
        }


        public static string DIR {
            get {
                var dir = Path.Combine(DataLocation.localApplicationData, "LOMProfiles");
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                return dir;
            }
        }

        public Mod[] Mods = new Mod[0];
        public Asset[] Assets = new Asset[0];
        public CO.DLC []ExcludedDLCs = new CO.DLC[0];

        [XmlIgnore]
        public string SkipFilePathFinal;
        public string SkipFilePath {
            get => FromFinalPath(SkipFilePathFinal);
            set => SkipFilePathFinal = ToFinalPath(value);
        }


        public Mod GetMod(string includedPath) => Mods.FirstOrDefault(m => m.IncludedPathFinal == includedPath);
        public Asset GetAsset(string includedPath) => Assets.FirstOrDefault(m => m.IncludedPathFinal == includedPath);

        public void Serialize(string path) {
            try {
                Log.Called();
                XmlSerializer ser = new XmlSerializer(typeof(LoadOrderProfile));
                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write)) {
                    ser.Serialize(fs, this);
                }
            } catch (Exception ex) { ex.Log(); }
        }

        public static LoadOrderProfile Deserialize(string path) {
            try {
                XmlSerializer ser = new XmlSerializer(typeof(LoadOrderProfile));
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read)) {
                    return ser.Deserialize(fs) as LoadOrderProfile;
                }
            } catch {
                return null;
            }
        }
    }
}
