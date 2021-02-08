using System.IO;
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
					assetFolder.Delete();
					// TODO: ExecutedActions.Push(assetFolder); // We need all data that was deleted inside the assetFolder class
				}

				// TODO: Should it be the scope of this command? I guess not.
				if (assetFolder.IsCompilable())
				{
					assetFolder.CreateCompiledFolder();
					// TODO: Add info to ExecutedAction
				}
			});
		}

		public void Undo()
		{
			Logger.Warn("Undo action is not implemented for this command yet.");
		}
	}
}