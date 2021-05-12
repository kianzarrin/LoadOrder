#pragma warning disable
namespace CO.IO {
    using LoadOrderShared;
    using LoadOrderTool;
    using Microsoft.Win32;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;
    using LoadOrderShared;
    using static CO.IO.PathUtils;

    public static class DataLocation {
        const string installLocationSubKey_ = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 255710";
        const string installLocationKey_ = "InstallLocation";
        const string SteamPathSubKey_ = @"Software\Valve\Steam";
        const string SteamPathKey_ = "SteamPath";

        //private static bool m_IsReady;

        private static bool m_IsEditor = false;

        public static bool isMacOSX = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
        public static bool isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        static DataLocation()
        {
            try {
                var sw = System.Diagnostics.Stopwatch.StartNew();
                var data = LoadOrderConfig.Deserialize(localApplicationData);
                Log.Info($"LoadOrderConfig.Deserialize took {sw.ElapsedMilliseconds}ms");

                try {
                    if (Util.IsGamePath(data?.GamePath)) {
                        GamePath = RealPath(data.GamePath);
                    } else if (Directory.Exists(GamePath)) {
                        using (RegistryKey key = Registry.LocalMachine.OpenSubKey(installLocationSubKey_)) {
                            GamePath = key?.GetValue(installLocationKey_) as string;
                            GamePath = RealPath(GamePath);
                        }
                    }
                } catch(Exception ex) {
                    Log.Exception(ex);
                }

                try {
                    if (Util.IsSteamPath(data?.SteamPath)) {
                        SteamPath = RealPath(data.SteamPath);
                    } else if (!Directory.Exists(SteamPath)) {
                        using (RegistryKey key = Registry.CurrentUser.OpenSubKey(SteamPathSubKey_)) {
                            SteamPath = key?.GetValue(SteamPathKey_) as string;
                            SteamPath = RealPath(SteamPath);
                        }
                    }
                } catch(Exception ex) {
                    Log.Exception(ex);
                }

                if (Util.IsWSPath(data?.WorkShopContentPath)) {
                    WorkshopContentPath = RealPath(data.WorkShopContentPath);
                }

                CalculatePaths();
                bool bGame = !string.IsNullOrEmpty(GamePath);
                bool bSteam = !string.IsNullOrEmpty(SteamPath);
                if (bGame && bSteam) return;

                using (var spd = new LoadOrderTool.UI.SelectPathsDialog()) {
                    if (bGame) spd.CitiesPath = GamePath;
                    if (bSteam) spd.SteamPath = SteamPath;
                    if (spd.ShowDialog() == DialogResult.OK) {
                        GamePath = Path.GetDirectoryName(spd.CitiesPath);
                        SteamPath = Path.GetDirectoryName(spd.SteamPath);
                        CalculatePaths();

                        data ??= new LoadOrderConfig();
                        data.GamePath = GamePath;
                        data.SteamPath = SteamPath;
                        data.WorkShopContentPath = WorkshopContentPath;
                        data.Serialize(localApplicationData);
                    } else {
                        Process.GetCurrentProcess().Kill();
                    }
                }


                if (!Directory.Exists(GamePath))
                    throw new Exception("failed to get GamePath : " + GamePath);
                if (!Directory.Exists(WorkshopContentPath))
                    throw new Exception("failed to get SteamContentPath : " + WorkshopContentPath);
            } catch (Exception ex) {
                Log.Exception(ex);
            }
        }

        private static void CalculatePaths() {
            string gamePath = GamePath, steamPath = SteamPath, wsPath = WorkshopContentPath;
            Util.CalculatePaths(ref gamePath, ref steamPath, ref wsPath);
            GamePath = RealPath(gamePath); 
            SteamPath = RealPath(steamPath);
            WorkshopContentPath = RealPath(wsPath);
        }

        //public static bool isEditor
        //{
        //	get
        //	{
        //		return DataLocation.m_IsEditor;
        //	}
        //	set
        //	{
        //		DataLocation.m_IsEditor = value;
        //		DataLocation.m_IsReady = true;
        //	}
        //}

        //private static void CheckReady()
        //{
        //	if (!DataLocation.m_IsReady)
        //	{
        //		throw new Exception("DataLocation is not ready to be used yet because the editor flag has not been set");
        //	}
        //}

        public static void DisplayStatus()
        {
            Log.Info("GamePath: " + DataLocation.GamePath);
            Log.Info("Workshop Content Path: " + DataLocation.WorkshopContentPath);
            Log.Info("Steam Path: " + DataLocation.SteamPath);
            Log.Info("Temp Folder: " + DataLocation.tempFolder);
            Log.Info("Local Application Data: " + DataLocation.localApplicationData);
            Log.Info("Executable Directory(Cities.exe): " + DataLocation.executableDirectory);
            Log.Info("Save Location: " + DataLocation.saveLocation);
            Log.Info("Application base: " + DataLocation.applicationBase);
            Log.Info("Addons path: " + DataLocation.addonsPath);
            Log.Info("Mods path: " + DataLocation.modsPath);
            //Log.Debug("Current directory: " + Environment.CurrentDirectory);
            //Log.Debug("Executing assembly: " + Assembly.GetExecutingAssembly().Location);
        }

        public static class Util {
            public static string ToGamePath(string path) {
                path = ToSteamAppsPath(path);
                path = Path.Combine(path, "common", "Cities_Skylines");
                return Directory.Exists(path) ? path : "";
            }

            public static string ToWSPath(string path) {
                path = ToSteamAppsPath(path);
                path = Path.Combine(path, "workshop", "content", "255710");
                return Directory.Exists(path) ? path : "";
            }
            public static string ToSteamPath(string path) => GoUpToDirectory(path, "Steam");

            public static string ToSteamAppsPath(string path) {
                try {
                    string ret = GoUpToDirectory(path, "steamapps");
                    if (!Directory.Exists(ret)) {
                        ret = Path.Combine(GoUpToDirectory(path, "Steam"), "steamapps");
                    }
                    return Directory.Exists(ret) ? ret : "";
                }catch(Exception ex) {
                    Log.Exception(ex);
                    return "";
                }
            }

            public static bool IsGamePath(string path) {
                if (string.IsNullOrEmpty(path))
                    return false;
                path = Path.Combine(path, "Cities.exe");
                return IsCitiesExePath(path);

            }
            public static bool IsSteamPath(string path) {
                if (string.IsNullOrEmpty(path))
                    return false;
                path = Path.Combine(path, "Steam.exe");
                return IsSteamExePath(path);
            }

            public static bool IsSteamExePath(string path) {
                return File.Exists(path) && string.Equals(Path.GetFileName(path), "Steam.exe", StringComparison.OrdinalIgnoreCase);
            }
            public static bool IsCitiesExePath(string path) {
                return File.Exists(path) && string.Equals(Path.GetFileName(path), "Cities.exe", StringComparison.OrdinalIgnoreCase);
            }

            public static bool IsWSPath(string path) {
                return Directory.Exists(path);
            }

            public static void CalculatePaths(ref string GamePath, ref string SteamPath, ref string WSPath) {
                try {
                    var paths = new[] { WSPath, GamePath, currentDirectory, assemblyDirectory, SteamPath };
                    foreach (string path in paths) {
                        if (IsWSPath(WSPath))
                            break;
                        WSPath = ToWSPath(path);
                    }
                    foreach (string path in paths) {
                        if (IsGamePath(GamePath))
                            break;
                        WSPath = ToGamePath(path);
                    }
                    foreach (string path in paths) {
                        if (IsSteamPath(SteamPath))
                            break;
                        SteamPath = ToWSPath(path);
                    }
                } catch (Exception ex) {
                    Log.Exception(ex);
                }
            }
        }




        public static string GamePath { get; private set; } = @"C:\Program Files (x86)\Steam\steamapps\common\Cities_Skylines";

        public static string WorkshopContentPath { get; private set; } = @"C:\Program Files (x86)\Steam\steamapps\workshop\content\255710";
        public static string SteamPath { get; private set; } = @"C:\Program Files (x86)\Steam";

        public static string applicationSupportPathName = "~/Library/Application Support/"; //mac


        public static string migrateProductFrom {
            set {
                string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Path.Combine(DataLocation.companyName, value));
                if (Directory.Exists(text)) {
                    string text2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Path.Combine(DataLocation.companyName, DataLocation.productName));
                    if (Directory.Exists(text2)) {
                        Log.Error(string.Concat(new string[]
                        {
                            "Migration from '",
                            text,
                            "' to '",
                            text2,
                            "' failed because new location already exists!"
                        }));
                        return;
                    }
                    Directory.Move(text, text2);
                }
            }
        }

        public const string sDevFolder = "Dev";

        public const string sProductName = "Colossal Order";

        public const string sCompanyName = "Cities_Skylines";
        public static string companyName => sProductName;
        public static string productName => sCompanyName;

        public static string applicationBase => GamePath;

        public static string gameContentPath {
            get {
                string text;
                if (isMacOSX) {
                    text = Path.Combine(Path.Combine(DataLocation.applicationBase, "Resources"), "Files");
                } else {
                    text = Path.Combine(DataLocation.applicationBase, "Files");
                }
                if (!Directory.Exists(text)) {
                    Directory.CreateDirectory(text);
                }
                return text;
            }
        }

        public static string addonsPath {
            get {
                string text = Path.Combine(DataLocation.localApplicationData, "Addons");
                if (!Directory.Exists(text)) {
                    Directory.CreateDirectory(text);
                }
                return text;
            }
        }

        public static string modsPath {
            get {
                string text = Path.Combine(DataLocation.addonsPath, "Mods");
                if (!Directory.Exists(text)) {
                    Directory.CreateDirectory(text);
                }
                return text;
            }
        }

        public static string assetsPath {
            get {
                string text = Path.Combine(DataLocation.addonsPath, "Assets");
                if (!Directory.Exists(text)) {
                    Directory.CreateDirectory(text);
                }
                return text;
            }
        }

        public static string mapThemesPath {
            get {
                string text = Path.Combine(DataLocation.addonsPath, "MapThemes");
                if (!Directory.Exists(text)) {
                    Directory.CreateDirectory(text);
                }
                return text;
            }
        }

        public static string stylesPath {
            get {
                string text = Path.Combine(DataLocation.addonsPath, "Styles");
                if (!Directory.Exists(text)) {
                    Directory.CreateDirectory(text);
                }
                return text;
            }
        }

        public static string currentDirectory {
            get {
                return Environment.CurrentDirectory;
            }
        }

        public static string assemblyDirectory {
            get {
                return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }
        }

        public static string DataPath => Path.Combine(GamePath, "Cities_Data");
        public static string ManagedDLL => Path.Combine(DataPath, "Managed");
        public static string MonoPath => Path.Combine(DataPath, "Mono");

        public static string BuiltInContentPath => Path.Combine(GamePath, "Files");
        public static string AssetStateSettingsFile => "userGameState";
        public static string executableDirectory = GamePath;


        public static string tempFolder {
            get {
                string text = Path.Combine(Path.GetTempPath(), Path.Combine(DataLocation.companyName, DataLocation.productName));
                if (!Directory.Exists(text)) {
                    Directory.CreateDirectory(text);
                }
                return text;
            }
        }

        public static string localApplicationData {
            get {
                if (isMacOSX) {
                    string text = Path.Combine(DataLocation.applicationSupportPathName, Path.Combine(DataLocation.companyName, DataLocation.productName));
                    if (!Directory.Exists(text)) {
                        Directory.CreateDirectory(text);
                    }
                    return text;
                }
                string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                if (isLinux) {
                    string environmentVariable = Environment.GetEnvironmentVariable("XDG_DATA_HOME");
                    if (!string.IsNullOrEmpty(environmentVariable)) {
                        path = environmentVariable;
                    }
                }
                string text2 = Path.Combine(path, Path.Combine(DataLocation.companyName, DataLocation.productName));
                if (!Directory.Exists(text2)) {
                    Directory.CreateDirectory(text2);
                }
                return text2;
            }
        }

        public static string saveLocation {
            get {
                string text = Path.Combine(DataLocation.localApplicationData, "Saves");
                if (!Directory.Exists(text)) {
                    Directory.CreateDirectory(text);
                }
                return text;
            }
        }

        public static string scenarioLocation {
            get {
                string text = Path.Combine(DataLocation.localApplicationData, "Scenarios");
                if (!Directory.Exists(text)) {
                    Directory.CreateDirectory(text);
                }
                return text;
            }
        }

        public static string mapLocation {
            get {
                string text = Path.Combine(DataLocation.localApplicationData, "Maps");
                if (!Directory.Exists(text)) {
                    Directory.CreateDirectory(text);
                }
                return text;
            }
        }
    }
}
