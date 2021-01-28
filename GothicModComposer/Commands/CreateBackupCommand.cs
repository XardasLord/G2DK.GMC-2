using System.Threading.Tasks;
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

		public Task Execute()
		{
			return Task.CompletedTask;
		}
	}
}