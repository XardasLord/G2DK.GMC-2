using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using GothicModComposer.Commands.ExecutedCommandActions;
using GothicModComposer.Commands.ExecutedCommandActions.Interfaces;
using GothicModComposer.Models.Profiles;
using GothicModComposer.Utils;
using GothicModComposer.Utils.IOHelpers;
using GothicModComposer.Utils.ProgressBar;
using ShellProgressBar;

namespace GothicModComposer.Commands
{
	public class CopyEssentialAssetFilesFromBackupCommand : ICommand
	{
		public string CommandName => "Copy essential asset files from backup (Preset, Music, Video, Scripts/_compiled)";

		private readonly IProfile _profile;
		private static readonly Stack<ICommandActionIO> ExecutedActions = new();

		public CopyEssentialAssetFilesFromBackupCommand(IProfile profile)
		{
			_profile = profile;
		}

		public void Execute()
		{
			var workDataBackupFiles = DirectoryHelper.GetAllFilesInDirectory(_profile.GmcFolder.BackupWorkDataFolderPath);
			var essentialFiles = workDataBackupFiles.FindAll(IsEssentialFile).ToList();

			Logger.Info($"Start copying all asset files from backup to {_profile.GothicFolder.WorkDataFolderPath} ...", true);

			using (var progress = new ProgressBar(essentialFiles.Count, "Copying asset files from backup", ProgressBarOptionsHelper.Get()))
			{
				var counter = 1;

				essentialFiles.ForEach(essentialFilePath =>
				{
					var relativePath = DirectoryHelper.ToRelativePath(essentialFilePath, _profile.GmcFolder.BackupWorkDataFolderPath);
					var destinationPath = DirectoryHelper.MergeRelativePath(_profile.GothicFolder.WorkDataFolderPath, relativePath);

					if (FileHelper.Exists(destinationPath))
					{
						var tmpCommandActionBackupPath =
							Path.Combine(_profile.GmcFolder.GetTemporaryCommandActionBackupPath(GetType().Name), Path.GetFileName(destinationPath));

						FileHelper.CopyWithOverwrite(destinationPath, tmpCommandActionBackupPath);
						FileHelper.CopyWithOverwrite(essentialFilePath, destinationPath);
						
						ExecutedActions.Push(CommandActionIO.FileCopiedWithOverwrite(destinationPath, tmpCommandActionBackupPath));
					}
					else
					{
						FileHelper.Copy(essentialFilePath, destinationPath);

						ExecutedActions.Push(CommandActionIO.FileCopied(essentialFilePath, destinationPath));
					}

					progress.Tick($"Copied {counter++} of {essentialFiles.Count} files");
				});
			}

			Logger.Info($"Copied all asset files from backup to {_profile.GothicFolder.WorkDataFolderPath}", true);
		}

		public void Undo() => ExecutedActions.Undo();

		private bool IsEssentialFile(string filePath)
		{
			var relativeFilePath = DirectoryHelper.ToRelativePath(filePath, _profile.GmcFolder.BackupWorkDataFolderPath);
			return _profile.GmcFolder.EssentialDirectoriesFiles.Any(folder => relativeFilePath.StartsWith(folder));
		}
	}
}