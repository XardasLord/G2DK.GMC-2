using System.Collections.Generic;
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

			FileHelper.CopyWithOverwrite(extensionFilePath, extensionRootPath);

			// TODO: Make a mechanism to somehow undo copy with overwrite, because this undo won't restore the original file that was overwritten
			// TODO: Maybe if we CopyWithOverwrite we should make a copy with suffix '_backup' or copy it into a .gmc folder in order to keep original file content if undo will be required?
			ExecutedActions.Push(CommandActionIO.FileCopied(extensionFilePath, extensionRootPath));
		}

		public void Undo() => ExecutedActions.Undo();
	}
}