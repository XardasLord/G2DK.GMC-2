namespace GothicModComposer.Core.Commands.ExecutedCommandActions.Interfaces
{
	/// <summary>
	///     Represents single IO command action executed inside ICommand
	/// </summary>
	public interface ICommandActionIO : ICommandAction
    {
        CommandActionIOType ActionType { get; }
        string SourcePath { get; }
        string DestinationPath { get; }
    }
}