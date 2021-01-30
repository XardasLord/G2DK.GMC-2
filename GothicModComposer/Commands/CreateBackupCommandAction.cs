namespace GothicModComposer.Commands
{
	public class CreateBackupCommandAction
	{
		public CommandActionType ActionType { get; }
		public string SourcePath { get; }
		public string DestinationPath { get; }

		public CreateBackupCommandAction(CommandActionType actionType, string sourcePath, string destinationPath)
		{
			ActionType = actionType;
			SourcePath = sourcePath;
			DestinationPath = destinationPath;
		}
	}

	public enum CommandActionType
	{
		FileCopy,
		FileMove,
		DirectoryCopy,
		DirectoryMove,
		DirectoryCreate
	}
}