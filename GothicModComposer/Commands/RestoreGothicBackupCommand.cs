using GothicModComposer.Models;
using GothicModComposer.Utils;
using GothicModComposer.Utils.IOHelpers;

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
			DirectoryHelper.DeleteIfExists(_gothicFolder.WorkDataFolderPath);

			DirectoryHelper.GetAllFilesInDirectory(_gmcFolder.BackupFolderPath).ForEach(backupFilePath =>
			{
				var relativePath = DirectoryHelper.ToRelativePath(backupFilePath, _gmcFolder.BackupFolderPath);
				var gothicFilePath = DirectoryHelper.MergeRelativePath(_gothicFolder.BasePath, relativePath);

				FileHelper.MoveWithOverwrite(backupFilePath, gothicFilePath);
			});
		}

		private void RemoveGmcFolder() => DirectoryHelper.DeleteIfExists(_gmcFolder.BasePath);

		public void Undo() => Logger.Warn("Undo of this command is not implemented.");
	}
}