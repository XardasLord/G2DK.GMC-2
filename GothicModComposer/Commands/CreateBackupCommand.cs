using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using GothicModComposer.Commands.ExecutedCommandActions;
using GothicModComposer.Commands.ExecutedCommandActions.Interfaces;
using GothicModComposer.Models.Profiles;
using GothicModComposer.Presets;
using GothicModComposer.Utils;
using GothicModComposer.Utils.ProgressBar;
using ShellProgressBar;

namespace GothicModComposer.Commands
{
    public class CreateBackupCommand : ICommand
	{
		public string CommandName => "Create original Gothic backup";

		private readonly IProfile _profile;
        private readonly IFileSystem _fileSystem;
        private static readonly Stack<ICommandActionIO> ExecutedActions = new();
		
		public CreateBackupCommand(IProfile profile, IFileSystem fileSystem)
        {
            _profile = profile;
            _fileSystem = fileSystem;
        }

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

					var sourcePath = _fileSystem.Path.Combine(_profile.GothicFolder.WorkDataFolderPath, assetFolder.ToString());
					var destinationPath = _fileSystem.Path.Combine(_profile.GmcFolder.BackupWorkDataFolderPath, assetFolder.ToString());
					
					if (!_fileSystem.Directory.Exists(sourcePath))
						return;
					
					_fileSystem.Directory.Move(sourcePath, destinationPath);
					//DirectoryHelper.Move(sourcePath, destinationPath);

					ExecutedActions.Push(CommandActionIO.DirectoryMoved(sourcePath, destinationPath));
				});
			};
		}

        private void BackupFilesOverridenByExtensions()
        {
			if (!_fileSystem.Directory.Exists(_profile.ModFolder.ExtensionsFolderPath))
				return;
            
            _fileSystem.Directory
                .EnumerateFiles(_profile.ModFolder.ExtensionsFolderPath, "*", SearchOption.AllDirectories)
                .ToList()
                .ForEach(BackupFileFromExtensionFolder);

            //    DirectoryHelper
            //.GetAllFilesInDirectory(_profile.ModFolder.ExtensionsFolderPath)
            //.ForEach(BackupFileFromExtensionFolder);
		}

		private void BackupFileFromExtensionFolder(string filePath)
		{
			var extensionFileRelativePath = _fileSystem.Path.GetRelativePath(_profile.ModFolder.ExtensionsFolderPath, filePath);
			var extensionFileGothicPath = _fileSystem.Path.Combine(_profile.GothicFolder.BasePath, extensionFileRelativePath);
			var extensionFileGmcBackupPath = _fileSystem.Path.Combine(_profile.GmcFolder.BackupFolderPath, extensionFileRelativePath);

			if (!_fileSystem.File.Exists(extensionFileGothicPath))
				return;

			var folderFromExtensionDirectory = _fileSystem.Path.GetDirectoryName(extensionFileGmcBackupPath);
			if (_fileSystem.Directory.Exists(folderFromExtensionDirectory))
				return;

            _fileSystem.Directory.CreateDirectory(folderFromExtensionDirectory);
			//DirectoryHelper.CreateIfDoesNotExist(folderFromExtensionDirectory);
			_fileSystem.File.Copy(extensionFileGothicPath, extensionFileGmcBackupPath);
			//FileHelper.Copy(extensionFileGothicPath, extensionFileGmcBackupPath);

			ExecutedActions.Push(CommandActionIO.FileCopied(extensionFileGothicPath, extensionFileGmcBackupPath));
		}
	}
}