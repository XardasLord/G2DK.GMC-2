namespace GothicModComposer.Commands.ExecutedCommandActions.Interfaces
{
	/// <summary>
	/// Represents single command action executed inside ICommand
	/// </summary>
	public interface ICommandAction 
	{
		void Undo();
	}
}