using GothicModComposer.Models.Profiles;
using GothicModComposer.Utils;

namespace GothicModComposer.Commands
{
	public class ClearGmcTemporaryFilesCommand : ICommand
	{
		private readonly IProfile _profile;
		public string CommandName => "Clear GMC temporary files";

		public ClearGmcTemporaryFilesCommand(IProfile profile) 
			=> _profile = profile;

		public void Execute() => _profile.GmcFolder.DeleteTemporaryFiles();

		public void Undo() => Logger.Info("Undo of this command is not available.", true);
	}
}