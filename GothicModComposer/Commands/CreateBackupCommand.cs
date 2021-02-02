﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GothicModComposer.Commands.ExecutedActions;
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

		private static Stack<IOCommandAction> ExecutedActions = new();
		
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

				switch (executedAction.ActionType)
				{
					case IOCommandActionType.FileCopy:
						FileHelper.CopyWithOverwrite(executedAction.DestinationPath, executedAction.SourcePath);
						break;
					case IOCommandActionType.FileMove:
						FileHelper.MoveWithOverwrite(executedAction.DestinationPath, executedAction.SourcePath);
						break;
					case IOCommandActionType.DirectoryCopy:
						DirectoryHelper.Copy(executedAction.DestinationPath, executedAction.SourcePath);
						break;
					case IOCommandActionType.DirectoryMove:
						DirectoryHelper.Move(executedAction.DestinationPath, executedAction.SourcePath);
						break;
					case IOCommandActionType.DirectoryCreate:
						DirectoryHelper.DeleteIfExists(executedAction.DestinationPath);
						break;
					case IOCommandActionType.FileCreate:
						FileHelper.DeleteIfExists(executedAction.DestinationPath);
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}

		private void BackupGothicWorkDataFolder()
		{
			_gmcFolder.CreateBackupWorkDataFolder();
			ExecutedActions.Push(new IOCommandAction(IOCommandActionType.DirectoryCreate, null, _gmcFolder.BasePath));

			AssetPresetFolders.FoldersWithAssets.ForEach(assetFolder =>
			{
				var sourcePath = Path.Combine(_gothicFolder.WorkDataFolderPath, assetFolder.ToString());
				var destinationPath = Path.Combine(_gmcFolder.BackupWorkDataFolderPath, assetFolder.ToString());

				if (!Directory.Exists(sourcePath))
					return;

				DirectoryHelper.Move(sourcePath, destinationPath);

				ExecutedActions.Push(new IOCommandAction(IOCommandActionType.DirectoryMove, sourcePath, destinationPath));
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

			ExecutedActions.Push(new IOCommandAction(IOCommandActionType.FileCopy, extensionFileGothicPath, extensionFileGmcBackupPath));
		}
	}
}