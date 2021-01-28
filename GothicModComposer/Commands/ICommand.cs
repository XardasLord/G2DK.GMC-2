using System.Threading.Tasks;

namespace GothicModComposer.Commands
{
	public interface ICommand
	{
		string CommandName { get; }

		Task Execute();
	}
}