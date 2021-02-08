using System.Collections.Generic;
using System.IO;
using GothicModComposer.Commands.ExecutedCommandActions;
using GothicModComposer.Commands.ExecutedCommandActions.Interfaces;
using GothicModComposer.Models.Folders;
using GothicModComposer.Models.Profiles;
using GothicModComposer.Presets;
using GothicModComposer.Utils;

namespace GothicModComposer.Commands
{
	public class ClearWorkDataCommand : ICommand
	{
		public string CommandName => "Clear Gothic '_Work/Data' folder";
		
		private readonly IProfile _profile;
		private static readonly Stack<ICommandActionIO> ExecutedActions = new();

		public ClearWorkDataCommand(IProfile profile) 
			=> _profile = profile;

		public void Execute()
		{
			AssetPresetFolders.FoldersWithAssets.ForEach(assetType =>
			{
				var assetFolderPath = Path.Combine(_profile.GothicFolder.WorkDataFolderPath, assetType.ToString());

				var assetFolder = new AssetFolder(assetFolderPath, assetType);
				if (assetFolder.Exists())
				{
					// TODO: ExecutedActions.Push(assetFolder); // We need all data that was deleted inside the assetFolder class
					ExecutedActions.Push(CommandActionIO.DirectoryDeleted(assetFolder.BasePath));

					assetFolder.Delete();
				}

				// TODO: Should it be the scope of this command? I guess not.
				if (assetFolder.IsCompilable())
				{
					assetFolder.CreateCompiledFolder();
					ExecutedActions.Push(CommandActionIO.DirectoryCreated(assetFolder.CompiledFolderPath));
				}
			});
		}

		public void Undo()
		{
			Logger.Warn("Undo action is not implemented for this command yet.");
		}
	}
}