using System.Collections.Generic;
using System.IO;
using System.Linq;
using GothicModComposer.Commands.ExecutedCommandActions;
using GothicModComposer.Commands.ExecutedCommandActions.Interfaces;
using GothicModComposer.Models;
using GothicModComposer.Presets;
using GothicModComposer.Utils;
using GothicModComposer.Utils.IOHelpers;

namespace GothicModComposer.Commands
{
	public class CreateBackupCommand : ICommand
	{
		public string CommandName => "Create original Gothic backup";

		private readonly GothicFolder _gothicFolder;
		private readonly GmcFolder _gmcFolder;
		private readonly ModFolder _modFolder;

		private static readonly Stack<ICommandActionIO> ExecutedActions = new();
		
		public CreateBackupCommand(GothicFolder gothicFolder, GmcFolder gmcFolder, ModFolder modFolder)
		{
			_gothicFolder = gothicFolder;
			_gmcFolder = gmcFolder;
			_modFolder = modFolder;
		}

		public void Execute()
		{
			if (_gmcFolder.DoesBackupFolderExist)
			{
				Logger.Info("Backup folder already exists.");
				return;
			}

			BackupGothicWorkDataFolder();
			BackupFilesOverridenByExtensions();
		}

		public void Undo()
		{
			if (!ExecutedActions.Any())
			{
				Logger.Info("There is nothing to undo, because no actions were executed.");
				return;
			}

			while (ExecutedActions.Count > 0)
			{
				var executedAction = ExecutedActions.Pop();
				executedAction?.Undo();
			}
		}

		private void BackupGothicWorkDataFolder()
		{
			_gmcFolder.CreateBackupWorkDataFolder();
			ExecutedActions.Push(CommandActionIO.DirectoryCreated(_gmcFolder.BasePath));

			AssetPresetFolders.FoldersWithAssets.ForEach(assetFolder =>
			{
				var sourcePath = Path.Combine(_gothicFolder.WorkDataFolderPath, assetFolder.ToString());
				var destinationPath = Path.Combine(_gmcFolder.BackupWorkDataFolderPath, assetFolder.ToString());

				if (!Directory.Exists(sourcePath))
					return;

				DirectoryHelper.Move(sourcePath, destinationPath);

				ExecutedActions.Push(CommandActionIO.DirectoryMoved(sourcePath, destinationPath));
			});
		}

		private void BackupFilesOverridenByExtensions()
			=> DirectoryHelper
				.GetAllFilesInDirectory(_modFolder.ExtensionsFolderPath)
				.ForEach(BackupFileFromExtensionFolder);

		private void BackupFileFromExtensionFolder(string filePath)
		{
			var extensionFileRelativePath = DirectoryHelper.ToRelativePath(filePath, _modFolder.ExtensionsFolderPath);
			var extensionFileGothicPath = DirectoryHelper.MergeRelativePath(_gothicFolder.BasePath, extensionFileRelativePath);
			var extensionFileGmcBackupPath = DirectoryHelper.MergeRelativePath(_gmcFolder.BackupFolderPath, extensionFileRelativePath);

			if (!File.Exists(extensionFileGothicPath))
				return;

			var folderFromExtensionDirectory = Path.GetDirectoryName(extensionFileGmcBackupPath);
			DirectoryHelper.CreateIfDoesNotExist(folderFromExtensionDirectory);
			FileHelper.CopyWithOverwrite(extensionFileGothicPath, extensionFileGmcBackupPath);

			ExecutedActions.Push(CommandActionIO.FileCopied(extensionFileGothicPath, extensionFileGmcBackupPath));
		}
	}
}