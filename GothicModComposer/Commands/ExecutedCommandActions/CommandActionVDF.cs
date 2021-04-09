using GothicModComposer.Commands.ExecutedCommandActions.Interfaces;
using GothicModComposer.Models.VdfFiles;
using GothicModComposer.Utils;

namespace GothicModComposer.Commands.ExecutedCommandActions
{
	public class CommandActionVDF : ICommandActionVDF
	{
		public CommandActionVDFType ActionType { get; }
		public VdfFile File { get; }

		private CommandActionVDF(CommandActionVDFType actionType, VdfFile file)
		{
			ActionType = actionType;
			File = file;
		}

		public static CommandActionVDF FileEnabled(VdfFile file)
			=> new CommandActionVDF(CommandActionVDFType.VdfEnabled, file);

		public static CommandActionVDF FileDisabled(VdfFile file)
			=> new CommandActionVDF(CommandActionVDFType.VdfDisabled, file);

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
	}
}