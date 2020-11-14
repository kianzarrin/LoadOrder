using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using LoadOrder;
using System.Runtime.InteropServices;

namespace COSettings.IO
{
	public static class DataLocation
	{
		public static string sProductName = "Cities_Skylines";

		public static string sCompanyName = "Colossal Order";

		public static uint sProductVersion = 0u;

		public static string sDevFolder = "Dev";

		//private static bool m_IsReady;

		private static bool m_IsEditor =false;

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
			Log.Debug("GamePath: " + DataLocation.GamePath);
			Log.Debug("Temp Folder: " + DataLocation.tempFolder);
			Log.Debug("Local Application Data: " + DataLocation.localApplicationData);
			Log.Debug("Executable Directory: " + DataLocation.executableDirectory);
			Log.Debug("Save Location: " + DataLocation.saveLocation);
			Log.Debug("Application base: " + DataLocation.applicationBase);
			Log.Debug("Addons path: " + DataLocation.addonsPath);
			Log.Debug("Mods path: " + DataLocation.modsPath);
		}

		public static string migrateProductFrom
		{
			set
			{
				string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Path.Combine(DataLocation.companyName, value));
				if (Directory.Exists(text))
				{
					string text2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Path.Combine(DataLocation.companyName, DataLocation.productName));
					if (Directory.Exists(text2))
					{
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

		public static string companyName
		{
			get
			{
				return DataLocation.sCompanyName;
			}
			set
			{
				DataLocation.sCompanyName = value;
			}
		}

		public static uint productVersion
		{
			get
			{
				return DataLocation.sProductVersion;
			}
			set
			{
				DataLocation.sProductVersion = value;
			}
		}

		public static string productVersionString
		{
			get
			{
				string text = "";
				uint num = DataLocation.sProductVersion % 100u;
				if (num != 0u)
				{
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

		public static string productName
		{
			get
			{
				return DataLocation.sProductName;
			}
			set
			{
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

		public static string gameContentPath
		{
			get
			{
				string text;
				if (isMacOSX)
				{
					text = Path.Combine(Path.Combine(DataLocation.applicationBase, "Resources"), "Files");
				}
				else
				{
					text = Path.Combine(DataLocation.applicationBase, "Files");
				}
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				return text;
			}
		}

		public static string addonsPath
		{
			get
			{
				string text = Path.Combine(DataLocation.localApplicationData, "Addons");
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				return text;
			}
		}

		public static string modsPath
		{
			get
			{
				string text = Path.Combine(DataLocation.addonsPath, "Mods");
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				return text;
			}
		}

		public static string assetsPath
		{
			get
			{
				string text = Path.Combine(DataLocation.addonsPath, "Assets");
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				return text;
			}
		}

		public static string mapThemesPath
		{
			get
			{
				string text = Path.Combine(DataLocation.addonsPath, "MapThemes");
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				return text;
			}
		}

		public static string stylesPath
		{
			get
			{
				string text = Path.Combine(DataLocation.addonsPath, "Styles");
				if (!Directory.Exists(text))
				{
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
		public static string GamePath => @"C:\Program Files (x86)\Steam\steamapps\common\Cities_Skylines";
		public static string SteamContentPath => @"C:\Program Files (x86)\Steam\steamapps\workshop\content\255710";
		public static string ManagedDLL => Path.Combine(GamePath, @"Cities_Data\Managed");
		public static string applicationSupportPathName = "~/Library/Application Support/"; //mac

		public static string BuiltInContentPath => Path.Combine(GamePath, "Files");
		public static string AssetStateSettingsFile => "userGameState";
		public static string DataPath => Path.Combine(GamePath, "Cities_Data");
		public static string executableDirectory = GamePath;
		

		public static string tempFolder
		{
			get
			{
				string text = Path.Combine(Path.GetTempPath(), Path.Combine(DataLocation.companyName, DataLocation.productName));
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				return text;
			}
		}

		public static string localApplicationData
		{
			get
			{
				if (isMacOSX)
				{
					string text = Path.Combine(DataLocation.applicationSupportPathName, Path.Combine(DataLocation.companyName, DataLocation.productName));
					if (!Directory.Exists(text))
					{
						Directory.CreateDirectory(text);
					}
					return text;
				}
				string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
				if (isLinux)
				{
					string environmentVariable = Environment.GetEnvironmentVariable("XDG_DATA_HOME");
					if (!string.IsNullOrEmpty(environmentVariable))
					{
						path = environmentVariable;
					}
				}
				string text2 = Path.Combine(path, Path.Combine(DataLocation.companyName, DataLocation.productName));
				if (!Directory.Exists(text2))
				{
					Directory.CreateDirectory(text2);
				}
				return text2;
			}
		}

		public static string saveLocation
		{
			get
			{
				string text = Path.Combine(DataLocation.localApplicationData, "Saves");
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				return text;
			}
		}

		public static string scenarioLocation
		{
			get
			{
				string text = Path.Combine(DataLocation.localApplicationData, "Scenarios");
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				return text;
			}
		}

		public static string mapLocation
		{
			get
			{
				string text = Path.Combine(DataLocation.localApplicationData, "Maps");
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				return text;
			}
		}
	}
}
