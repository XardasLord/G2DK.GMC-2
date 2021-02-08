using System.IO;
using GothicModComposer.Utils.IOHelpers;

namespace GothicModComposer.Models.Folders
{
	public class GmcFolder
	{
		public string BasePath { get; }
		public string BackupFolderPath => Path.Combine(BasePath, "backup");
		public string LogsFolderPath => Path.Combine(BasePath, "Logs");
		public string BuildFolderPath => Path.Combine(BasePath, "Build");
		public string VdsfConfigFilePath => Path.Combine(BasePath, "vdfsConfig");
		public string ModFilesTrackerFilePath => Path.Combine(BasePath, "modFiles.json");
		public string BackupWorkDataFolderPath => Path.Combine(BackupFolderPath, "_Work", "Data");

		public bool DoesBackupFolderExist => Directory.Exists(BackupFolderPath);

		public string EssentialFilesRegexPattern => @"((Presets|Music|Video))|[\/\\](Fonts|_intern)";

		private GmcFolder(string gmcFolderPath)
			=> BasePath = gmcFolderPath;

		public static GmcFolder CreateFromPath(string gmcFolderPath)
		{
			var instance = new GmcFolder(gmcFolderPath);

			instance.Verify();

			return instance;
		}

		public void CreateBackupWorkDataFolder() => DirectoryHelper.CreateIfDoesNotExist(BackupWorkDataFolderPath);

		private void Verify()
		{
			// TODO: Verify if the folder exists and is correct
		}
	}
}