using GothicModComposer.Commands.ExecutedCommandActions.Interfaces;
using GothicModComposer.Models;
using GothicModComposer.Utils;

namespace GothicModComposer.Commands.ExecutedCommandActions
{
    public class CommandActionVideoBik : ICommandActionVideoBik
    {
        private CommandActionVideoBik(CommandActionVideoBikType actionType, VideoBikFile file)
        {
            ActionType = actionType;
            File = file;
        }

        public CommandActionVideoBikType ActionType { get; }
        public VideoBikFile File { get; }

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

        public static CommandActionVideoBik FileEnabled(VideoBikFile file)
            => new(CommandActionVideoBikType.VideoBikEnabled, file);

        public static CommandActionVideoBik FileDisabled(VideoBikFile file)
            => new(CommandActionVideoBikType.VideoBikDisabled, file);
    }
}