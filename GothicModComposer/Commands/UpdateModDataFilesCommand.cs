﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using GothicModComposer.Commands.ExecutedCommandActions;
using GothicModComposer.Commands.ExecutedCommandActions.Interfaces;
using GothicModComposer.Models.ModFiles;
using GothicModComposer.Models.Profiles;
using GothicModComposer.Presets;
using GothicModComposer.Utils;
using GothicModComposer.Utils.IOHelpers;
using GothicModComposer.Utils.ProgressBar;
using ShellProgressBar;

namespace GothicModComposer.Commands
{
	public class UpdateModDataFilesCommand : ICommand
	{
		public string CommandName => "Update mod data files";

		private readonly IProfile _profile;
		private static readonly Stack<ICommandActionIO> ExecutedActions = new();
		private readonly Dictionary<ModFileEntry, ModFileEntryOperation> _modFileActions = new();

		public UpdateModDataFilesCommand(IProfile profile) 
			=> _profile = profile;

		public void Execute()
		{
			Logger.Info($"Start copying all mod files to {_profile.GothicFolder.WorkDataFolderPath}", true);

			var filesFromTrackerFileHelper = _profile.GmcFolder.GetModFilesFromTrackerFile();
			var modFiles = _profile.ModFolder.GetAllModFiles();

			// TODO: Refactor
			using (var parentProgressBar = new ProgressBar(3, "Updating mod files", ProgressBarOptionsHelper.Get()))
			{
				modFiles.ForEach(modFile =>
				{
					var modAsset = filesFromTrackerFileHelper.Find(x => x.RelativePath.Equals(modFile.RelativePath));

					var operation = modAsset?.GetEntryOperationForFile(modFile.FilePath) ?? ModFileEntryOperation.Create;

					if (operation == ModFileEntryOperation.None)
						return;

					_modFileActions.Add(modFile, operation);
				});

				parentProgressBar.Tick();

				// --------------------------

				var counter = 1;
				var modFilesToCreate = _modFileActions.Where(x => x.Value == ModFileEntryOperation.Create).ToList();

				using var childProgressBarForCreate = parentProgressBar.Spawn(modFilesToCreate.Count, "Creating new mod files", ProgressBarOptionsHelper.Get());

				foreach (var modFileAction in modFilesToCreate)
				{
					_profile.GmcFolder.AddNewModFileEntryToTrackerFile(modFileAction.Key);
					CopyAssetToCompilationFolder(modFileAction.Key);
					ApplyBuildConfigParameters(modFileAction.Key);

					childProgressBarForCreate.Tick($"Created {counter++} of {modFilesToCreate.Count} new files");
				}

				parentProgressBar.Tick();

				// --------------------------

				counter = 1;
				var modFilesToUpdate = _modFileActions.Where(x => x.Value == ModFileEntryOperation.Update).ToList();

				using var childProgressBarForUpdate = parentProgressBar.Spawn(modFilesToUpdate.Count, "Updating existing mod files", ProgressBarOptionsHelper.Get());

				foreach (var modFileAction in modFilesToUpdate)
				{
					_profile.GmcFolder.UpdateModFileEntryInTrackerFile(modFileAction.Key);
					CopyAssetToCompilationFolder(modFileAction.Key);
					DeleteCompiledAssetIfExists(modFileAction.Key);
					ApplyBuildConfigParameters(modFileAction.Key);

					childProgressBarForUpdate.Tick($"Updated {counter++} of {modFilesToUpdate.Count} existing files");
				}

				parentProgressBar.Tick();

				// TODO: In the future we should add next step to delete also files that has been deleted from the DK repository and still exists in the game mod files
			}

			Logger.Info($"Copied all mod files to {_profile.GothicFolder.WorkDataFolderPath}", true);

			_profile.GmcFolder.SaveTrackerFile();

			Logger.Info($"Created mod tracker file in {_profile.GmcFolder.ModFilesTrackerFilePath}",true);
			ExecutedActions.Push(CommandActionIO.FileCreated(_profile.GmcFolder.ModFilesTrackerFilePath));
		}

		public void Undo() => ExecutedActions.Undo();

		private void CopyAssetToCompilationFolder(ModFileEntry modFileEntry)
		{
			var gothicWorkDataFile = DirectoryHelper.MergeRelativePath(_profile.GothicFolder.WorkDataFolderPath, modFileEntry.RelativePath);

			if (FileHelper.Exists(gothicWorkDataFile))
			{
				var tmpCommandActionBackupPath =
					Path.Combine(_profile.GmcFolder.GetTemporaryCommandActionBackupPath(GetType().Name), Path.GetFileName(gothicWorkDataFile));

				FileHelper.CopyWithOverwrite(gothicWorkDataFile, tmpCommandActionBackupPath);
				FileHelper.CopyWithOverwrite(modFileEntry.FilePath, gothicWorkDataFile);

				ExecutedActions.Push(CommandActionIO.FileCopiedWithOverwrite(gothicWorkDataFile, tmpCommandActionBackupPath));
			}
			else
			{
				FileHelper.Copy(modFileEntry.FilePath, gothicWorkDataFile);

				ExecutedActions.Push(CommandActionIO.FileCopied(modFileEntry.FilePath, gothicWorkDataFile));
			}
		}

		private void DeleteCompiledAssetIfExists(ModFileEntry modFile)
		{
			var assetFolderPath = Path.Combine(_profile.GothicFolder.WorkDataFolderPath, modFile.AssetType.ToString());

			var compiledFileName = modFile.GetCompiledFileName();

			if (compiledFileName is null)
				return;

			var pathToCompiledFile = Path.Combine(assetFolderPath, "_compiled", compiledFileName);

			if (!FileHelper.Exists(pathToCompiledFile))
				return;

			var tmpCommandActionBackupPath = Path.Combine(_profile.GmcFolder.GetTemporaryCommandActionBackupPath(GetType().Name), "_compiled", compiledFileName);

			FileHelper.CopyWithOverwrite(pathToCompiledFile, tmpCommandActionBackupPath);
			FileHelper.DeleteIfExists(pathToCompiledFile);

			ExecutedActions.Push(CommandActionIO.FileDeleted(pathToCompiledFile, tmpCommandActionBackupPath));
		}

		private void ApplyBuildConfigParameters(ModFileEntry modFileEntry)
		{
			switch (modFileEntry.AssetType)
			{
				case AssetPresetType.Anims:
				case AssetPresetType.Meshes:
					_profile.GothicArguments.ZConvertAll();
					break;
				case AssetPresetType.Scripts:
					_profile.GothicArguments.ZReparse();
					break;
				case AssetPresetType.Textures:
					_profile.GothicArguments.ZTexConvert();
					break;
			}
		}
	}
}
