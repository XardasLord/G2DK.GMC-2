using GothicModComposer.Core.Models;

namespace GothicModComposer.Core.Commands.ExecutedCommandActions.Interfaces
{
    /// <summary>
    ///     Represents single Video BIK file command action executed inside ICommand
    /// </summary>
    public interface ICommandActionVideoBik : ICommandAction
    {
        CommandActionVideoBikType ActionType { get; }
        VideoBikFile File { get; }
    }
}