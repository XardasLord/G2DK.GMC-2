using GothicModComposer.Core.Models.VdfFiles;

namespace GothicModComposer.Core.Commands.ExecutedCommandActions.Interfaces
{
	/// <summary>
	///     Represents single VDF file command action executed inside ICommand
	/// </summary>
	public interface ICommandActionVDF : ICommandAction
    {
        CommandActionVDFType ActionType { get; }
        VdfFile File { get; }
    }
}