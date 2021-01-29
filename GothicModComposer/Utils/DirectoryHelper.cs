using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GothicModComposer.Utils
{
	public static class DirectoryHelper
	{
		public static string ToRelativePath(string fullPath, string basePath) 
			=> fullPath.Replace(basePath, "");

		public static string MergeRelativePath(string relativePath, string toAdd) 
			=> Path.GetFullPath($"{relativePath}{toAdd}");

		public static List<string> GetAllFilesInDirectory(string directory, SearchOption searchOption = SearchOption.AllDirectories) 
			=> Directory.Exists(directory) 
				? Directory.EnumerateFiles(directory, "*", searchOption).ToList()
				: new List<string>();

		public static void CreateDirectoryIfDoesNotExist(string path)
		{
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
		}

		public static void MoveDirectory(string source, string dest)
		{
			Directory.Move(source, dest);
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
		}

		public static bool DeleteDirectoryIfExists(string path)
		{
			if (!Directory.Exists(path)) 
				return false;
			
			Directory.Delete(path, true);
			return true;
		}
	}
}