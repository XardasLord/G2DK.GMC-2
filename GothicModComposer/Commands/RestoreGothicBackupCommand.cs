using GothicModComposer.Models.Profiles;
using GothicModComposer.Utils;
using GothicModComposer.Utils.IOHelpers;
using GothicModComposer.Utils.ProgressBar;
using ShellProgressBar;

namespace GothicModComposer.Commands
{
	public class RestoreGothicBackupCommand : ICommand
	{
		public string CommandName => "Restore original Gothic backup files";

		private readonly IProfile _profile;

		public RestoreGothicBackupCommand(IProfile profile) 
			=> _profile = profile;

		public void Execute()
		{
			if (!_profile.GmcFolder.DoesBackupFolderExist)
			{
				Logger.Info("There is no backup folder to restore.", true);
				return;
			}

			RestoreBackup();
			RemoveGmcFolder();
		}

		private void RestoreBackup()
		{
			DirectoryHelper.DeleteIfExists(_profile.GothicFolder.WorkDataFolderPath);

			var backupFiles = DirectoryHelper.GetAllFilesInDirectory(_profile.GmcFolder.BackupFolderPath);

			using (var progress = new ProgressBar(backupFiles.Count, "Restoring files from backup", ProgressBarOptionsHelper.Get()))
			{
				var counter = 1;

				backupFiles.ForEach(backupFilePath =>
				{
					var relativePath = DirectoryHelper.ToRelativePath(backupFilePath, _profile.GmcFolder.BackupFolderPath);
					var gothicFilePath = DirectoryHelper.MergeRelativePath(_profile.GothicFolder.BasePath, relativePath);

					FileHelper.MoveWithOverwrite(backupFilePath, gothicFilePath);

					progress.Tick($"Restored {counter} of {backupFiles.Count} files");
				});
			}
			
		}

		private void RemoveGmcFolder() => DirectoryHelper.DeleteIfExists(_profile.GmcFolder.BasePath);

		public void Undo() => Logger.Warn("Undo of this command is not implemented.");
	}
}