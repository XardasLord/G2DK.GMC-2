namespace GothicModComposer.Commands.ExecutedActions
{
	public class IOCommandAction
	{
		public IOCommandActionType ActionType { get; }
		public string SourcePath { get; }
		public string DestinationPath { get; }

		public IOCommandAction(IOCommandActionType actionType, string sourcePath, string destinationPath)
		{
			ActionType = actionType;
			SourcePath = sourcePath;
			DestinationPath = destinationPath;
		}
	}
}