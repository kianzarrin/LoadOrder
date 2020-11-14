using System;
using System.IO;

namespace COSettings.IO
{
	public static class DataLocation
	{
		public static string sProductName = "Cities_Skylines";
		public static string sCompanyName = "Colossal Order";
		public static bool isMacOSX = Environment.OSVersion.Platform == PlatformID.MacOSX;
		public static bool isLinux = Environment.OSVersion.Platform == PlatformID.Unix;

		public static string applicationSupportPathName = "~/Library/Application Support/"; //mac

		public static string localApplicationData
		{
			get
			{
				if (isMacOSX)
				{
					string text = Path.Combine(applicationSupportPathName, Path.Combine(sCompanyName, sProductName));
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
				string text2 = Path.Combine(path, Path.Combine(sCompanyName, sProductName));
				if (!Directory.Exists(text2))
				{
					Directory.CreateDirectory(text2);
				}
				return text2;
			}
		}
	}
}
