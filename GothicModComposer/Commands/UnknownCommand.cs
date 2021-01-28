using System.Threading.Tasks;

namespace GothicModComposer.Commands
{
	public class UnknownCommand : ICommand
	{
		public string CommandName => "Unknown command";

		public Task Execute() => Task.CompletedTask;
	}
}