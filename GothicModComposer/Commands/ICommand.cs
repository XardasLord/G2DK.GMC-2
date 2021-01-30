namespace GothicModComposer.Commands
{
	public interface ICommand
	{
		string CommandName { get; }

		void Execute();
		void Undo();
	}
}