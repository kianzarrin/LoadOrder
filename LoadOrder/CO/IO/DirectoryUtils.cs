using System;
using System.IO;

namespace CO.IO
{
	public class DirectoryUtils
	{
		public static void DeleteDirectory(string path)
		{
			foreach (string path2 in Directory.GetDirectories(path))
			{
				DirectoryUtils.DeleteDirectory(path2);
			}
			try
			{
				Directory.Delete(path, true);
			}
			catch (IOException)
			{
				Directory.Delete(path, true);
			}
			catch (UnauthorizedAccessException)
			{
				Directory.Delete(path, true);
			}
		}
	}
}