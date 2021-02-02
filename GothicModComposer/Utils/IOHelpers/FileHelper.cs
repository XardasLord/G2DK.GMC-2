using System.IO;
using System.Text;

namespace GothicModComposer.Utils.IOHelpers
{
	public static class FileHelper
	{
		public static bool Exists(string path)
			=> File.Exists(path);

		public static void CopyWithOverwrite(string source, string dest)
		{
			if (File.Exists(dest))
			{
				File.Copy(source, dest, true);
			}
			else
			{
				CreateFileAlongWithMissingDirectories(dest);
				File.Copy(source, dest, true);
			}

			Logger.Info($"Copied file \"{source}\" ---> \"{dest}\".");
		}

		public static void MoveWithOverwrite(string source, string dest)
		{
			if (File.Exists(dest))
			{
				File.Move(source, dest, true);
			}
			else
			{
				CreateFileAlongWithMissingDirectories(dest);
				File.Move(source, dest, true);
			}

			Logger.Info($"Moved file \"{source}\" ---> \"{dest}\".");
		}

		public static bool DeleteIfExists(string path)
		{
			if (!File.Exists(path))
				return false;

			File.Delete(path);

			Logger.Info($"Deleted file \"{path}\".");
			return true;
		}

		public static void SaveContent(string path, string content, Encoding encoding) 
			=> File.WriteAllText(path, content, encoding);

		private static void CreateFileAlongWithMissingDirectories(string dest) 
			=> new FileInfo(dest).Directory?.Create();
	}
}
