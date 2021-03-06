using System.Collections.Generic;
using System.IO;
using System.Linq;
using SearchOption = System.IO.SearchOption;

namespace GothicModComposer.Utils.IOHelpers
{
	public static class DirectoryHelper
	{
		public static bool Exists(string path)
			=> Directory.Exists(path);

		public static string ToRelativePath(string fullPath, string relativeTo)
			=> Path.GetRelativePath(relativeTo, fullPath);

		public static string MergeRelativePath(string relativePath, string toAdd)
			=> Path.Combine(relativePath, toAdd);

		public static List<string> GetAllFilesInDirectory(string directoryPath, SearchOption searchOption = SearchOption.AllDirectories) 
			=> Directory.Exists(directoryPath) 
				? Directory.EnumerateFiles(directoryPath, "*", searchOption).ToList()
				: new List<string>();

		public static List<string> GetDirectories(string directoryPath)
			=> Directory.GetDirectories(directoryPath).ToList();

		public static bool CreateIfDoesNotExist(string path)
		{
			if (Directory.Exists(path))
				return false;
			
			Directory.CreateDirectory(path);

			Logger.Info($"Created directory \"{path}\".");
			return true;
		}

		public static bool DeleteIfExists(string path)
		{
			if (!Directory.Exists(path))
				return false;

			Directory.Delete(path, true);

			Logger.Info($"Deleted directory \"{path}\".");
			return true;
		}

		public static void Copy(string source, string dest)
		{
            Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(source, dest);

			Logger.Info($"Copied directory \"{source}\" ---> \"{dest}\".");
		}

		public static void Move(string source, string dest)
		{
			Directory.Move(source, dest);

			Logger.Info($"Moved directory \"{source}\" ---> \"{dest}\".");
		}
	}
}