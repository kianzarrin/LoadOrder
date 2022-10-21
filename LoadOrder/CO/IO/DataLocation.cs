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

        public static bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        public static bool isMacOSX = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
        public static bool isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        static DataLocation() {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            string m = "Delayed messages: "; // delayed message;
            CSCache csCache = null;
            try {
                csCache = CSCache.Deserialize();
                if(csCache == null) {
                    csCache = new CSCache();
                    // backward compatibility.
                    var data2 = LoadOrderConfig.Deserialize();
                    if(data2 != null) {
                        csCache.WorkShopContentPath = data2.WorkShopContentPath;
                        csCache.GamePath = data2.GamePath;
                        csCache.SteamPath = data2.SteamPath;
                    }
                }

                sw.Stop();

                try {
                    m += "\ndata is " + (csCache is null ? "null" : "not null");
                    m += $"\ndata?.GamePath={csCache?.GamePath ?? "<null>"}";
                    if (Util.IsGamePath(csCache?.GamePath)) {
                        m += "\ngame path found: " + csCache.GamePath;
                        GamePath = RealPath(csCache.GamePath);
                        m += "\nreal game path is: " + GamePath;
                    } else if (!Util.IsGamePath(GamePath)) {
                        m += "\ngetting game path from registry ...";
                        using (RegistryKey key = Registry.LocalMachine.OpenSubKey(installLocationSubKey_)) {
                            GamePath = key?.GetValue(installLocationKey_) as string;
                            if (Util.IsGamePath(GamePath)) {
                                GamePath = RealPath(GamePath);
                            } else {
                                GamePath = null;
                            }
                            m += "\ngame path from registry: " + GamePath;
                        }
                    }
                    m += "\n[P1] game path so far is:" + GamePath;
                } catch (Exception ex) {
                    Log.Exception(ex);
                }

                try {
                    m += $"\ndata?.SteamPath={csCache?.SteamPath ?? "<null>"}";
                    if (Util.IsSteamPath(csCache?.SteamPath)) {
                        m += "\nSteamPath found: " + csCache.SteamPath;
                        SteamPath = RealPath(csCache.SteamPath);
                        m += "\nreal SteamPath is: " + SteamPath;
                    } else if (!Util.IsSteamPath(SteamPath)) {
                        m += "\ngetting SteamPath from registry ...";
                        using (RegistryKey key = Registry.CurrentUser.OpenSubKey(SteamPathSubKey_)) {
                            SteamPath = key?.GetValue(SteamPathKey_) as string;
                            if (Util.IsSteamPath(csCache?.SteamPath)) {
                                SteamPath = RealPath(SteamPath);
                            } else {
                                SteamPath = null;
                            }
                            m += "\nSteamPath from registry: " + SteamPath;
                        }
                    }
                    m += "\n[P2] SteamPath so far is:" + SteamPath;
                } catch (Exception ex) {
                    Log.Exception(ex);
                }

                m += $"\ndata?.WorkShopContentPath={csCache?.WorkShopContentPath ?? "<null>"}";
                if (Util.IsWSPath(csCache?.WorkShopContentPath)) {
                    WorkshopContentPath = RealPath(csCache.WorkShopContentPath);
                    m += "\nWorkshopContentPath found: " + WorkshopContentPath;
                }
                m += "\n[P3]WorkshopContentPath so far is: " + WorkshopContentPath;

                CalculatePaths();
                m += "\n[P4] AfterCalucaltePaths";
                m += "\nGamePath=" + (GamePath ?? "<null>");
                m += "\nSteamPath=" + (SteamPath ?? "<null>");
                m += "\nWorkshopContentPath=" + (WorkshopContentPath ?? "<null>");
                bool bGame = !string.IsNullOrEmpty(GamePath);
                bool bSteam = !string.IsNullOrEmpty(SteamPath);
                if (bGame && bSteam) return;

                m += "\n[P5] Creating select path dialog";
                using (var spd = new LoadOrderTool.UI.SelectPathsDialog()) {
                    if (bGame) spd.CitiesPath = GamePath;
                    if (bSteam) spd.SteamPath = SteamPath;
                    if (spd.ShowDialog() == DialogResult.OK) {
                        GamePath = Path.GetDirectoryName(spd.CitiesPath);
                        SteamPath = Path.GetDirectoryName(spd.SteamPath);
                        CalculatePaths();
                        m += "\n[P5] AfterCalucaltePaths";
                        m += "\nGamePath=" + (GamePath ?? "<null>");
                        m += "\nSteamPath=" + (SteamPath ?? "<null>");
                        m += "\nWorkshopContentPath=" + (WorkshopContentPath ?? "<null>");

                        csCache ??= new CSCache();
                        csCache.GamePath = GamePath;
                        csCache.SteamPath = SteamPath;
                        csCache.WorkShopContentPath = WorkshopContentPath;
                        csCache.Serialize();
                    } else {
                        Log.Info(m);
                        Process.GetCurrentProcess().Kill();
                    }
                }

                m += "\n[P6] at the end:";
                m += "\nGamePath=" + (GamePath ?? "<null>");
                m += "\nSteamPath=" + (SteamPath ?? "<null>");
                m += "\nWorkshopContentPath=" + (WorkshopContentPath ?? "<null>");

                if (!Directory.Exists(GamePath))
                    throw new Exception("failed to get GamePath : " + GamePath);
                if (!Directory.Exists(WorkshopContentPath))
                    throw new Exception("failed to get SteamContentPath : " + WorkshopContentPath);
            } catch (Exception ex) {
                Log.Exception(ex);
            } finally {
                Log.Info(m);
                VerifyPaths();
                if (csCache != null) ValidatePathMatch(csCache: csCache);
                Log.Debug($"LoadOrderConfig.Deserialize took {sw.ElapsedMilliseconds}ms");
                DataLocation.DisplayStatus();
            }
        }

        private static void CalculatePaths() {
            string gamePath = GamePath, steamPath = SteamPath, wsPath = WorkshopContentPath;
            Util.CalculatePaths(ref gamePath, ref steamPath, ref wsPath);
            GamePath = Util.IsGamePath(gamePath) ? RealPath(gamePath) : "";
            SteamPath = Util.IsSteamPath(steamPath) ? RealPath(steamPath) : "";
            WorkshopContentPath = Util.IsWSPath(wsPath) ? RealPath(wsPath) : "";
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

        public static void DisplayStatus() {
            Log.Info("GamePath: " + DataLocation.GamePath);
            Log.Info("Workshop Content Path: " + DataLocation.WorkshopContentPath);
            Log.Info("Steam Path: " + DataLocation.SteamPath);
            Log.Info("Temp Folder: " + DataLocation.tempFolder);
            Log.Info("Local Application Data: " + DataLocation.localApplicationData);
            Log.Info($"Executable Directory({DataLocation.CitiesExe}): " + DataLocation.executableDirectory);
            Log.Info("Save Location: " + DataLocation.saveLocation);
            Log.Info("Application base: " + DataLocation.applicationBase);
            Log.Info("Addons path: " + DataLocation.addonsPath);
            Log.Info("Mods path: " + DataLocation.modsPath);
            //Log.Debug("Current directory: " + Environment.CurrentDirectory);
            //Log.Debug("Executing assembly: " + Assembly.GetExecutingAssembly().Location);
        }

        public static string PrintPaths() {
            string ret = "GamePath: " + DataLocation.GamePath;
            ret += "\nWorkshop Content Path: " + DataLocation.WorkshopContentPath;
            ret += "\nSteam Path: " + DataLocation.SteamPath;
            ret += "\nTemp Folder: " + DataLocation.tempFolder;
            ret += "\nLocal Application Data: " + DataLocation.localApplicationData;
            ret += $"\nExecutable Directory({DataLocation.CitiesExe}): " + DataLocation.executableDirectory;
            ret += "\nSave Location: " + DataLocation.saveLocation;
            ret += "\nApplication base: " + DataLocation.applicationBase;
            ret += "\nAddons path: " + DataLocation.addonsPath;
            ret += "\nMods path: " + DataLocation.modsPath;
            ret += "\nCurrent directory: " + DataLocation.currentDirectory;
            return ret;
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
                } catch (Exception ex) {
                    Log.Exception(ex);
                    return "";
                }
            }

            public static bool IsGamePath(string path) {
                if (string.IsNullOrEmpty(path))
                    return false;
                path = Path.Combine(path, DataLocation.CitiesExe);
                return IsCitiesExePath(path);
            }

            public static bool IsSteamPath(string path) {
                if (string.IsNullOrEmpty(path))
                    return false;
                path = Path.Combine(path, DataLocation.SteamExe);
                return IsSteamExePath(path);
            }

            public static bool IsSteamExePath(string path) {
                return File.Exists(path) && string.Equals(Path.GetFileName(path), DataLocation.SteamExe, StringComparison.OrdinalIgnoreCase);
            }
            public static bool IsCitiesExePath(string path) {
                return File.Exists(path) && string.Equals(Path.GetFileName(path), DataLocation.CitiesExe, StringComparison.OrdinalIgnoreCase);
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

        public static void ValidatePathMatch(CSCache csCache) {
            foreach (var mod in csCache.ItemTable.Values.OfType<CSCache.Mod>()) {
                string WSDirName = "255710";
                int i = mod.IncludedPath.IndexOf(WSDirName);
                if (i > 0) {
                    string WSDirPath = mod.IncludedPath.Substring(0, i + WSDirName.Length);
                    string m =
                        $"tool uses '{WorkshopContentPath}'\n" +
                        $"game uses '{WSDirPath}'";
                    Log.Debug(m);
                    if (WorkshopContentPath != WSDirPath) {
                        new Exception($"Path mismatch! enabling/disabling may not work.\n" + m).Log();
                    }
                }
            }
        }

        public static bool VerifyPaths() {
            try {
                bool good =
                    Util.IsGamePath(GamePath) &&
                    Util.IsWSPath(WorkshopContentPath) &&
                    Util.IsSteamPath(SteamPath);
                if (good)
                    return true;
                MessageBox.Show("could not find paths\n" + PrintPaths());
            } catch (Exception ex) {
                MessageBox.Show("could not find paths\n" + PrintPaths()); ;
                ex.Log();
            }
            return false;
        }

        public static string CitiesExe {
            get {
                if (isWindows)
                    return "Cities.exe";
                else if (isLinux)
                    return "Cities.x64";
                else if (isMacOSX)
                    return "Cities";
                else
                    return "Cities"; // unknown platform.
            }
        }
        public static string SteamExe {
            get {
                if (isWindows)
                    return "Steam.exe";
                else if (isLinux)
                    return "Steam";
                else if (isMacOSX)
                    return "Steam";
                else
                    return "Steam"; // unknown platform.
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
                return currentDirectory; // work around: application is put inside temp folder so we can't get the path to original assembly.
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

        public static string LocalLOMData {
            get {
                var localAppData = DataLocation.localApplicationData;
                var ret = Path.Combine(localAppData, "LoadOrder");
                try {
                    if (!Directory.Exists(ret)) {
                        Directory.CreateDirectory(ret);

                        // move files first time for backward compatibility.
                        TryMoveFile("LoadOrderConfig.xml");
                        TryMoveFile("LoadOrderToolSettings.xml");
                        TryMoveDir("LOMProfiles");

                        void TryMoveFile(string fileName) {
                            var source = Path.Combine(localAppData, fileName);
                            var dest = Path.Combine(ret, fileName);
                            if (File.Exists(source)) {
                                File.Move(source, dest, false);
                            }
                        }
                        void TryMoveDir(string dirName) {
                            var source = Path.Combine(localAppData, dirName);
                            var dest = Path.Combine(ret, dirName);
                            if (Directory.Exists(source) && !Directory.Exists(dest)) {
                                Directory.Move(source, dest);
                            }
                        }
                    }
                } catch(Exception ex) { ex.Log(); }
                return ret;
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
