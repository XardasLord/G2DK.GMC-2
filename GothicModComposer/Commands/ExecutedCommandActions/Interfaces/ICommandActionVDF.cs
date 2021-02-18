using GothicModComposer.Models.VdfFiles;

namespace GothicModComposer.Commands.ExecutedCommandActions.Interfaces
{
	/// <summary>
	/// Represents single VDF file command action executed inside ICommand
	/// </summary>
	public interface ICommandActionVDF : ICommandAction
	{
		CommandActionVDFType ActionType { get; }
		VdfFile File { get; }
	}

	public enum CommandActionVDFType
	{
		VdfEnabled,
		VdfDisabled
	}
}