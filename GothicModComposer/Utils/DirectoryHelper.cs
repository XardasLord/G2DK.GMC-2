using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualBasic.FileIO;
using SearchOption = System.IO.SearchOption;

namespace GothicModComposer.Utils
{
	public static class DirectoryHelper
	{
		public static string ToRelativePath(string fullPath, string basePath) 
			=> fullPath.Replace(basePath, "");

		public static string MergeRelativePath(string relativePath, string toAdd) 
			=> Path.GetFullPath($"{relativePath}{toAdd}");

		public static List<string> GetAllFilesInDirectory(string directoryPath, SearchOption searchOption = SearchOption.AllDirectories) 
			=> Directory.Exists(directoryPath) 
				? Directory.EnumerateFiles(directoryPath, "*", searchOption).ToList()
				: new List<string>();

		public static void CreateDirectoryIfDoesNotExist(string path)
		{
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);

				Logger.Info($"Created directory \"{path}\".");
			}
		}

		public static void CopyDirectory(string source, string dest)
		{
			FileSystem.CopyDirectory(source, dest);

			Logger.Info($"Copied directory \"{source}\" ---> \"{dest}\".");
		}

		public static void MoveDirectory(string source, string dest)
		{
			Directory.Move(source, dest);

			Logger.Info($"Moved directory \"{source}\" ---> \"{dest}\".");
		}

		public static void CopyFileWithOverwrite(string source, string dest)
		{
			if (File.Exists(dest))
			{
				File.Copy(source, dest, true);
			}
			else
			{
				new FileInfo(dest).Directory?.Create();
				File.Copy(source, dest, true);
			}

			Logger.Info($"Copied file \"{source}\" ---> \"{dest}\".");
		}

		public static void MoveFileWithOverwrite(string source, string dest)
		{
			if (File.Exists(dest))
			{
				File.Move(source, dest, true);
			}
			else
			{
				new FileInfo(dest).Directory?.Create();
				File.Move(source, dest, true);
			}

			Logger.Info($"Moved file \"{source}\" ---> \"{dest}\".");
		}

		public static bool DeleteDirectoryIfExists(string path)
		{
			if (!Directory.Exists(path)) 
				return false;
			
			Directory.Delete(path, true);

			Logger.Info($"Deleted directory \"{path}\".");
			return true;
		}
	}
}