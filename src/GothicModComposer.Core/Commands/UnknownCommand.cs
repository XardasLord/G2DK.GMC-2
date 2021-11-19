namespace GothicModComposer.Core.Commands
{
    public class UnknownCommand : ICommand
    {
        public string CommandName => "Unknown";

        public async Task ExecuteAsync()
        {
        }

        public void Undo()
        {
        }
    }
}