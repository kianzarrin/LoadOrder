using System;
using System.IO;

namespace COSettings.IO
{
	public class PathUtils
	{
		public static string AddTimeStamp(string fileName)
		{
			string directoryName = Path.GetDirectoryName(fileName);
			string path = Path.GetFileNameWithoutExtension(fileName) + DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(fileName);
			return Path.Combine(directoryName, path);
		}

		public static string MakePath(string path)
		{
			return path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
		}

		public static string MakeUniquePath(string path)
		{
			string directoryName = Path.GetDirectoryName(path);
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
			string extension = Path.GetExtension(path);
			int num = 1;
			while (File.Exists(path))
			{
				path = Path.Combine(directoryName, string.Concat(new object[]
				{
					fileNameWithoutExtension,
					" ",
					num,
					extension
				}));
				num++;
			}
			return path;
		}

		public static string AddExtension(string path, string ext)
		{
			if (string.IsNullOrEmpty(Path.GetExtension(path)))
			{
				path += ext;
			}
			else
			{
				path = Path.ChangeExtension(path, ext);
			}
			return path;
		}
	}
}
