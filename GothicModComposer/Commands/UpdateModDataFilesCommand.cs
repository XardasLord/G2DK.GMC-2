using System.Collections.Generic;
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

		public UpdateModDataFilesCommand(IProfile profile) 
			=> _profile = profile;

		public void Execute()
		{
			Logger.Info($"Start copying all mod files to {_profile.GothicFolder.WorkDataFolderPath} ...", true);

			var filesFromTrackerFileHelper = _profile.GmcFolder.GetModFilesFromTrackerFile();
			var modFiles = _profile.ModFolder.GetAllModFiles();

			using (var progress = new ProgressBar(modFiles.Count, "Updating mod files", ProgressBarOptionsHelper.Get()))
			{
				var counter = 1;

				modFiles.ForEach(modFile =>
				{
					progress.Tick($"Copied {counter++} of {modFiles.Count} files");

					var modAsset = filesFromTrackerFileHelper.Find(x => x.RelativePath.Equals(modFile.RelativePath));

					var operation = modAsset?.GetEntryOperationForFile(modFile.FilePath) ?? ModFileEntryOperation.Create;
					switch (operation)
					{
						case ModFileEntryOperation.Create:
							_profile.GmcFolder.AddNewModFileEntryToTrackerFile(modFile);
							CopyAssetToCompilationFolder(modFile);
							ApplyBuildConfigParameters(modFile);
							return;
						case ModFileEntryOperation.Update:
							_profile.GmcFolder.UpdateModFileEntryInTrackerFile(modFile);
							CopyAssetToCompilationFolder(modFile);
							ApplyBuildConfigParameters(modFile);
							return;
						case ModFileEntryOperation.None:
							return;
					}
				});
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
