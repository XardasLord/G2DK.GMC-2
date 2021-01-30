using System.IO;

namespace GothicModComposer.Utils.IOHelpers
{
	public static class FileHelper
	{
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

		private static void CreateFileAlongWithMissingDirectories(string dest) 
			=> new FileInfo(dest).Directory?.Create();
	}
}
