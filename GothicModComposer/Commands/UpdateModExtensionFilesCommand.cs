using System.Collections.Generic;
using System.IO;
using GothicModComposer.Commands.ExecutedCommandActions;
using GothicModComposer.Commands.ExecutedCommandActions.Interfaces;
using GothicModComposer.Models.Profiles;
using GothicModComposer.Utils.IOHelpers;

namespace GothicModComposer.Commands
{
	public class UpdateModExtensionFilesCommand : ICommand
	{
		public string CommandName => "Update mod extension files";

		private readonly IProfile _profile;
		private static readonly Stack<ICommandActionIO> ExecutedActions = new();

		public UpdateModExtensionFilesCommand(IProfile profile)
			=> _profile = profile;

		public void Execute()
		{
			DirectoryHelper
				.GetAllFilesInDirectory(_profile.ModFolder.ExtensionsFolderPath)
				.ForEach(UpdateExtensionFile);
		}

		private void UpdateExtensionFile(string extensionFilePath)
		{
			var extensionRelativePath = DirectoryHelper.ToRelativePath(extensionFilePath, _profile.ModFolder.ExtensionsFolderPath);
			var extensionRootPath = DirectoryHelper.MergeRelativePath(_profile.GothicFolder.BasePath, extensionRelativePath);

			if (FileHelper.Exists(extensionRootPath))
			{
				var tmpCommandActionBackupPath = 
					Path.Combine(_profile.GmcFolder.GetTemporaryCommandActionBackupPath(GetType().Name), Path.GetFileName(extensionRootPath));

				FileHelper.Copy(extensionRootPath, tmpCommandActionBackupPath);
				FileHelper.CopyWithOverwrite(extensionFilePath, extensionRootPath);

				ExecutedActions.Push(CommandActionIO.FileCopiedWithOverwrite(extensionRootPath, tmpCommandActionBackupPath));
			}
			else
			{
				FileHelper.Copy(extensionFilePath, extensionRootPath);
				
				ExecutedActions.Push(CommandActionIO.FileCopied(extensionFilePath, extensionRootPath));
			}
		}

		public void Undo() => ExecutedActions.Undo();
	}
}