#pragma warning disable
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

namespace CO.IO {
    public static class DataLocation {
        static string installLocationSubKey_ = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 255710";
        static string installLocationKey_ = "InstallLocation";
        static string SteamPathSubKey_ = @"Software\Valve\Steam";
        static string SteamPathKey_ = "SteamPath";

        public static string RealPath(string path)
        {
            if (!Directory.Exists(path)) {
                Log.Exception(new DirectoryNotFoundException("path"));
                return null;
            }
            try {
                path = Path.GetFullPath(path);
                var ret = Path.GetPathRoot(path).ToUpper();
                foreach (var name in path.Substring(ret.Length).Split(Path.DirectorySeparatorChar)) {
                    var entries = Directory.GetFileSystemEntries(ret);
                    ret = entries.First(
                        p => string.Equals(Path.GetFileName(p), name, StringComparison.OrdinalIgnoreCase));
                }
                //Log.Debug("returning " + root);
                return ret;
            } catch (Exception ex) {
                Log.Exception(ex, "failed to get real path for: " + path);
                return null;
            }
        }

        static DataLocation()
        {
            try {
                var sw = System.Diagnostics.Stopwatch.StartNew();
                var data = LoadOrderConfig.Deserialize(localApplicationData);
                Log.Info($"LoadOrderConfig.Deserialize took {sw.ElapsedMilliseconds}ms");

                if (Directory.Exists(GamePath)) {
                    // good :)
                } else if (Directory.Exists(data?.GamePath)) { 
                    GamePath = data.GamePath;
                } else { 
                    using (RegistryKey key = Registry.LocalMachine.OpenSubKey(installLocationSubKey_)) {
                        GamePath = key?.GetValue(installLocationKey_) as string;
                        GamePath = RealPath(GamePath);
                    }
                }

                if (Directory.Exists(WorkshopContentPath)) {
                    // good :)
                } else if (Directory.Exists(data?.WorkShopContentPath)) {
                    WorkshopContentPath = data.WorkShopContentPath;
                } else {
                    using (RegistryKey key = Registry.CurrentUser.OpenSubKey(SteamPathSubKey_)) {
                        string steamPath = key?.GetValue(SteamPathKey_) as string;
                        WorkshopContentPath = Path.Combine(steamPath, @"steamapps\workshop\content\255710");
                        WorkshopContentPath = RealPath(WorkshopContentPath);
                    }
                }

                bool bGame = Directory.Exists(GamePath);
                bool bSteam = Directory.Exists(WorkshopContentPath);
                if (bGame && bSteam) return;

                using (var spd = new LoadOrderTool.UI.SelectPathsDialog()) {
                    if (bGame) spd.GamePath = GamePath;
                    if (bSteam) spd.WorkshopContentPath = WorkshopContentPath;
                    if (spd.ShowDialog() == DialogResult.OK) {
                        GamePath = spd.GamePath;
                        WorkshopContentPath = spd.WorkshopContentPath;

                        data ??= new LoadOrderConfig();
                        data.GamePath = GamePath;
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

        public static string sProductName = "Cities_Skylines";

        public static string sCompanyName = "Colossal Order";

        public static uint sProductVersion = 0u;

        public static string sDevFolder = "Dev";

        //private static bool m_IsReady;

        private static bool m_IsEditor = false;

        public static bool isMacOSX = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
        public static bool isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
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
            Log.Info("Steam Content Path: " + DataLocation.WorkshopContentPath);
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

        public static string companyName {
            get {
                return DataLocation.sCompanyName;
            }
            set {
                DataLocation.sCompanyName = value;
            }
        }

        public static uint productVersion {
            get {
                return DataLocation.sProductVersion;
            }
            set {
                DataLocation.sProductVersion = value;
            }
        }

        public static string productVersionString {
            get {
                string text = "";
                uint num = DataLocation.sProductVersion % 100u;
                if (num != 0u) {
                    text = ((char)(97u + num)).ToString();
                }
                return string.Format("{0}.{1}.{2}{3}", new object[]
                {
                    DataLocation.sProductVersion / 1000000u,
                    DataLocation.sProductVersion / 10000u % 100u,
                    DataLocation.sProductVersion / 100u % 100u,
                    text
                });
            }
        }

        public static string productName {
            get {
                return DataLocation.sProductName;
            }
            set {
                DataLocation.sProductName = value;
            }
        }

        public static string applicationBase => GamePath;
        //{
        //	get
        //	{
        //		DataLocation.CheckReady();
        //		if (DataLocation.m_IsEditor)
        //		{
        //			return GamePath;
        //		}
        //		return DataLocation.executableDirectory;
        //	}
        //}

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

        //public static string currentDirectory
        //{
        //	get
        //	{
        //		return Environment.CurrentDirectory;
        //	}
        //}

        //public static string assemblyDirectory
        //{
        //	get
        //	{
        //		return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        //	}
        //}


        //TODO: make platform independant.
        public static string GamePath { get; private set; } = @"C:\Program Files (x86)\Steam\steamapps\common\Cities_Skylines";
        public static string DataPath => Path.Combine(GamePath, "Cities_Data");
        public static string ManagedDLL => Path.Combine(DataPath, "Managed");
        public static string WorkshopContentPath { get; private set; } = @"C:\Program Files (x86)\Steam\steamapps\workshop\content\255710";
        public static string applicationSupportPathName = "~/Library/Application Support/"; //mac

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
