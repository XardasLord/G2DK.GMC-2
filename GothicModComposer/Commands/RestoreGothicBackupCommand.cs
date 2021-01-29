using System.IO;
using GothicModComposer.Models;
using GothicModComposer.Utils;

namespace GothicModComposer.Commands
{
	public class RestoreGothicBackupCommand : ICommand
	{
		public string CommandName => "Restore original Gothic backup files";

		private readonly GothicFolder _gothicFolder;
		private readonly GmcFolder _gmcFolder;

		public RestoreGothicBackupCommand(GothicFolder gothicFolder, GmcFolder gmcFolder)
		{
			_gothicFolder = gothicFolder;
			_gmcFolder = gmcFolder;
		}

		public void Execute()
		{
			if (!_gmcFolder.DoesBackupFolderExist)
			{
				Logger.Info("There is no backup folder to restore.");
				return;
			}

			RestoreBackup();
			RemoveGmcFolder();
		}

		private void RestoreBackup()
		{
			DirectoryHelper.DeleteDirectoryIfExists(_gothicFolder.WorkDataFolderPath);

			DirectoryHelper.GetAllFilesInDirectory(_gmcFolder.BackupFolderPath).ForEach(backupFilePath =>
			{
				var relativePath = DirectoryHelper.ToRelativePath(backupFilePath, _gmcFolder.BackupFolderPath);
				var gothicFilePath = DirectoryHelper.MergeRelativePath(_gothicFolder.BasePath, relativePath);

				DirectoryHelper.MoveFileWithOverwrite(backupFilePath, gothicFilePath);

				Logger.Info($"Restored file from backup \"{backupFilePath}\" ---> \"{gothicFilePath}\".");
			});
		}

		private void RemoveGmcFolder()
		{
			Directory.Delete(_gmcFolder.BasePath, true);

			Logger.Info("Removed .gmc directory.");
		}

		public void Revert()
		{
			throw new System.NotImplementedException();
		}
	}
}