using System;
using System.Collections.Generic;
using GothicModComposer.Commands.ExecutedCommandActions;
using GothicModComposer.Commands.ExecutedCommandActions.Interfaces;
using GothicModComposer.Models.ModFiles;
using GothicModComposer.Models.Profiles;
using GothicModComposer.Presets;
using GothicModComposer.Utils;
using GothicModComposer.Utils.IOHelpers;

namespace GothicModComposer.Commands
{
	public class UpdateModDataCommand : ICommand
	{
		public string CommandName => "Update mod data files";

		private readonly IProfile _profile;
		private static readonly Stack<ICommandActionIO> ExecutedActions = new();

		public UpdateModDataCommand(IProfile profile) 
			=> _profile = profile;

		public void Execute()
		{
			Logger.Info($"Start copying all mod files to {_profile.GothicFolder.WorkDataFolderPath} ...", true);

			var filesFromTrackerFileHelper = _profile.GmcFolder.GetModFilesFromTrackerFile();

			_profile.ModFolder.GetAllModFiles().ForEach(modFile =>
			{
				var modAsset = filesFromTrackerFileHelper.Find(x => x.RelativePath.Equals(modFile.RelativePath));

				var operation = modAsset?.GetEntryOperation(modFile.FilePath) ?? ModFileEntryOperation.Create;
				switch (operation)
				{
					case ModFileEntryOperation.None:
						return;
					case ModFileEntryOperation.Create:
						AddNewModFileEntry(modFile);
						return;
					case ModFileEntryOperation.Update:
						UpdateNewModFileEntry(modFile);
						return;
					default:
						throw new ArgumentOutOfRangeException(nameof(operation), "Unknown mod file entry operation type.");
				}
			});

			Logger.Info($"Copied all mod files to {_profile.GothicFolder.WorkDataFolderPath}", true);

			_profile.GmcFolder.SaveTrackerFile();

			Logger.Info($"Created mod tracker file in {_profile.GmcFolder.ModFilesTrackerFilePath}",true);
			ExecutedActions.Push(CommandActionIO.FileCreated(_profile.GmcFolder.ModFilesTrackerFilePath));
		}

		public void Undo() => ExecutedActions.Undo();

		private void AddNewModFileEntry(ModFileEntry modFileEntry)
		{
			_profile.GmcFolder.AddNewModFileEntryToTrackerFile(modFileEntry);
			CopyAssetToCompilationFolder(modFileEntry);
			ApplyBuildConfigParameters(modFileEntry);
		}

		private void UpdateNewModFileEntry(ModFileEntry modFileEntry)
		{
			modFileEntry.Timestamp = FileHelper.GetFileTimestamp(modFileEntry.FilePath); // TODO: Move to ModFileEntry class
			CopyAssetToCompilationFolder(modFileEntry);
			ApplyBuildConfigParameters(modFileEntry);
		}

		private void CopyAssetToCompilationFolder(ModFileEntry modFileEntry)
		{
			var gothicWorkDataFile = DirectoryHelper.MergeRelativePath(_profile.GothicFolder.WorkDataFolderPath, modFileEntry.RelativePath);
			FileHelper.CopyWithOverwrite(modFileEntry.FilePath, gothicWorkDataFile);

			ExecutedActions.Push(CommandActionIO.FileCopied(modFileEntry.FilePath, gothicWorkDataFile));
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
