using GothicModComposer.Models;

namespace GothicModComposer.Commands
{
	public class CreateBackupCommand : ICommand
	{
		public string CommandName => "Create backup";

		private readonly GothicFolder _gothicFolder;
		private readonly GmcFolder _gmcFolder;

		public CreateBackupCommand(GothicFolder gothicFolder, GmcFolder gmcFolder)
		{
			_gothicFolder = gothicFolder;
			_gmcFolder = gmcFolder;
		}

		public void Execute()
		{
		}

		public void Revert()
		{
		}
	}
}