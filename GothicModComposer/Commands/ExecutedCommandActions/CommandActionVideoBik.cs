using GothicModComposer.Commands.ExecutedCommandActions.Interfaces;
using GothicModComposer.Models;
using GothicModComposer.Utils;

namespace GothicModComposer.Commands.ExecutedCommandActions
{
    public class CommandActionVideoBik : ICommandActionVideoBik
    {
        public CommandActionVideoBikType ActionType { get; }
        public VideoBikFile File { get; }

        private CommandActionVideoBik(CommandActionVideoBikType actionType, VideoBikFile file)
        {
            ActionType = actionType;
            File = file;
        }

        public static CommandActionVideoBik FileEnabled(VideoBikFile file)
            => new CommandActionVideoBik(CommandActionVideoBikType.VideoBikEnabled, file);

        public static CommandActionVideoBik FileDisabled(VideoBikFile file)
            => new CommandActionVideoBik(CommandActionVideoBikType.VideoBikDisabled, file);

        public void Undo()
        {
            switch (ActionType)
            {
                case CommandActionVideoBikType.VideoBikEnabled:
                    File.Disable();
                    break;
                case CommandActionVideoBikType.VideoBikDisabled:
                    File.Enable();
                    break;
                default:
                    Logger.Warn("Unknown type of a single command action.");
                    break;
            }
        }
    }
}