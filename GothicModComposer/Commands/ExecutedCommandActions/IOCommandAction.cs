using System;
using GothicModComposer.Utils.IOHelpers;

namespace GothicModComposer.Commands.ExecutedCommandActions
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

		public void Undo()
		{
			switch (ActionType)
			{
				case IOCommandActionType.FileCopy:
					FileHelper.CopyWithOverwrite(DestinationPath, SourcePath);
					break;
				case IOCommandActionType.FileMove:
					FileHelper.MoveWithOverwrite(DestinationPath, SourcePath);
					break;
				case IOCommandActionType.DirectoryCopy:
					DirectoryHelper.Copy(DestinationPath, SourcePath);
					break;
				case IOCommandActionType.DirectoryMove:
					DirectoryHelper.Move(DestinationPath, SourcePath);
					break;
				case IOCommandActionType.DirectoryCreate:
					DirectoryHelper.DeleteIfExists(DestinationPath);
					break;
				case IOCommandActionType.FileCreate:
					FileHelper.DeleteIfExists(DestinationPath);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}