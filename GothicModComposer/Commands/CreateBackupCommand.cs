using System.Collections.Generic;
using System.IO;
using GothicModComposer.Commands.ExecutedCommandActions;
using GothicModComposer.Commands.ExecutedCommandActions.Interfaces;
using GothicModComposer.Models.Profiles;
using GothicModComposer.Presets;
using GothicModComposer.Utils;
using GothicModComposer.Utils.IOHelpers;
using GothicModComposer.Utils.ProgressBar;
using ShellProgressBar;

namespace GothicModComposer.Commands
{
	public class CreateBackupCommand : ICommand
	{
		public string CommandName => "Create original Gothic backup";

		private readonly IProfile _profile;
		private static readonly Stack<ICommandActionIO> ExecutedActions = new();
		
		public CreateBackupCommand(IProfile profile)
			=> _profile = profile;

		public void Execute()
		{
			if (_profile.GmcFolder.DoesBackupFolderExist)
			{
				Logger.Info("Backup folder already exists.", true);
				return;
			}

			BackupGothicWorkDataFolder();
			BackupFilesOverridenByExtensions();
		}

		public void Undo() => ExecutedActions.Undo();

		private void BackupGothicWorkDataFolder()
		{
			_profile.GmcFolder.CreateBackupWorkDataFolder();
			ExecutedActions.Push(CommandActionIO.DirectoryCreated(_profile.GmcFolder.BasePath));

			using (var progress = new ProgressBar(AssetPresetFolders.FoldersWithAssets.Count, "Creating Gothic backup", ProgressBarOptionsHelper.Get()))
			{
				var counter = 1;

				AssetPresetFolders.FoldersWithAssets.ForEach(assetFolder =>
				{
					progress.Tick($"Copied {counter++} of {AssetPresetFolders.FoldersWithAssets.Count} folders");

					var sourcePath = Path.Combine(_profile.GothicFolder.WorkDataFolderPath, assetFolder.ToString());
					var destinationPath = Path.Combine(_profile.GmcFolder.BackupWorkDataFolderPath, assetFolder.ToString());

					if (!Directory.Exists(sourcePath))
						return;

					DirectoryHelper.Move(sourcePath, destinationPath);

					ExecutedActions.Push(CommandActionIO.DirectoryMoved(sourcePath, destinationPath));
				});
			};
		}

		private void BackupFilesOverridenByExtensions()
			=> DirectoryHelper
				.GetAllFilesInDirectory(_profile.ModFolder.ExtensionsFolderPath)
				.ForEach(BackupFileFromExtensionFolder);

		private void BackupFileFromExtensionFolder(string filePath)
		{
			var extensionFileRelativePath = DirectoryHelper.ToRelativePath(filePath, _profile.ModFolder.ExtensionsFolderPath);
			var extensionFileGothicPath = DirectoryHelper.MergeRelativePath(_profile.GothicFolder.BasePath, extensionFileRelativePath);
			var extensionFileGmcBackupPath = DirectoryHelper.MergeRelativePath(_profile.GmcFolder.BackupFolderPath, extensionFileRelativePath);

			if (!File.Exists(extensionFileGothicPath))
				return;

			var folderFromExtensionDirectory = Path.GetDirectoryName(extensionFileGmcBackupPath);
			DirectoryHelper.CreateIfDoesNotExist(folderFromExtensionDirectory);
			FileHelper.Copy(extensionFileGothicPath, extensionFileGmcBackupPath);

			ExecutedActions.Push(CommandActionIO.FileCopied(extensionFileGothicPath, extensionFileGmcBackupPath));
		}
	}
}