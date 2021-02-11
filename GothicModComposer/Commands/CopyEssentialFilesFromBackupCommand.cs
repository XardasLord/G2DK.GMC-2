using System.Collections.Generic;
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
	public class CopyEssentialFilesFromBackupCommand : ICommand
	{
		public string CommandName => "Copy essential files from backup";

		private readonly IProfile _profile;
		private readonly Regex _essentialFileRegex;
		private static readonly Stack<ICommandActionIO> ExecutedActions = new();

		public CopyEssentialFilesFromBackupCommand(IProfile profile)
		{
			_profile = profile;
			_essentialFileRegex = new Regex(_profile.GmcFolder.EssentialFilesRegexPattern);
		}

		public void Execute()
		{
			var workDataBackupFiles = DirectoryHelper.GetAllFilesInDirectory(_profile.GmcFolder.BackupWorkDataFolderPath);
			var essentialFiles = workDataBackupFiles.FindAll(IsEssential).ToList();

			Logger.Info($"Start copying all asset files from backup to {_profile.GothicFolder.WorkDataFolderPath} ...", true);

			using (var progress = new ProgressBar(essentialFiles.Count, "Copying asset files from backup", ProgressBarOptionsHelper.Get()))
			{
				var counter = 1;

				essentialFiles.ForEach(essentialFilePath =>
				{
					var relativePath = DirectoryHelper.ToRelativePath(essentialFilePath, _profile.GmcFolder.BackupWorkDataFolderPath);
					var destinationPath = DirectoryHelper.MergeRelativePath(_profile.GothicFolder.WorkDataFolderPath, relativePath);

					FileHelper.CopyWithOverwrite(essentialFilePath, destinationPath);

					ExecutedActions.Push(CommandActionIO.FileCopied(essentialFilePath, destinationPath));

					progress.Tick($"Copied {counter++} of {essentialFiles.Count} files");
				});
			}

			Logger.Info($"Copied all asset files from backup to {_profile.GothicFolder.WorkDataFolderPath}", true);
		}

		public void Undo() => ExecutedActions.Undo();

		private bool IsEssential(string filePath)
		{
			var relativeFilePath = DirectoryHelper.ToRelativePath(filePath, _profile.GmcFolder.BackupWorkDataFolderPath);
			return _essentialFileRegex.IsMatch(relativeFilePath);
		}
	}
}