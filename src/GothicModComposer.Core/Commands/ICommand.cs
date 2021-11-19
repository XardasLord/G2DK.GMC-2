namespace GothicModComposer.Core.Commands
{
    public interface ICommand
    {
        string CommandName { get; }

        Task ExecuteAsync();
        void Undo();
    }
}