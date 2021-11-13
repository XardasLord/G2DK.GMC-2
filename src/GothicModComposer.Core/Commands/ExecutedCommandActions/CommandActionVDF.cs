using GothicModComposer.Core.Commands.ExecutedCommandActions.Interfaces;
using GothicModComposer.Core.Models.VdfFiles;
using GothicModComposer.Core.Utils;

namespace GothicModComposer.Core.Commands.ExecutedCommandActions
{
    public class CommandActionVDF : ICommandActionVDF
    {
        private CommandActionVDF(CommandActionVDFType actionType, VdfFile file)
        {
            ActionType = actionType;
            File = file;
        }

        public CommandActionVDFType ActionType { get; }
        public VdfFile File { get; }

        public void Undo()
        {
            switch (ActionType)
            {
                case CommandActionVDFType.VdfEnabled:
                    File.Disable();
                    break;
                case CommandActionVDFType.VdfDisabled:
                    File.Enable();
                    break;
                default:
                    Logger.Warn("Unknown type of a single command action.");
                    break;
            }
        }

        public static CommandActionVDF FileEnabled(VdfFile file)
            => new(CommandActionVDFType.VdfEnabled, file);

        public static CommandActionVDF FileDisabled(VdfFile file)
            => new(CommandActionVDFType.VdfDisabled, file);
    }
}