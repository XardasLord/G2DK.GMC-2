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

		public static void CopyFileWithOverwriteAndKeepOriginalFileWithSuffixAdded(string source, string dest, string suffixToAddToOriginalFile = "_overwrite")
		{
			if (File.Exists(dest))
			{
				File.Move(dest, $"{dest}{suffixToAddToOriginalFile}");
				File.Copy(source, dest);
			}
			else
			{
				new FileInfo(dest).Directory?.Create();
				File.Copy(source, dest, true);
			}
		}
	}
}