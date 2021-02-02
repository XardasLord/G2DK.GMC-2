using System;
using GothicModComposer.Commands.ExecutedCommandActions.Interfaces;
using GothicModComposer.Utils.IOHelpers;

namespace GothicModComposer.Commands.ExecutedCommandActions
{
	public class CommandActionIO : ICommandActionIO
	{
		public CommandActionIOType ActionType { get; }
		public string SourcePath { get; }
		public string DestinationPath { get; }

		public CommandActionIO(CommandActionIOType actionType, string sourcePath, string destinationPath)
		{
			ActionType = actionType;
			SourcePath = sourcePath;
			DestinationPath = destinationPath;
		}
		
		// TODO: Implement static factory for better handling inputs from the caller

		public void Undo()
		{
			switch (ActionType)
			{
				case CommandActionIOType.FileCopy:
					FileHelper.CopyWithOverwrite(DestinationPath, SourcePath);
					break;
				case CommandActionIOType.FileMove:
					FileHelper.MoveWithOverwrite(DestinationPath, SourcePath);
					break;
				case CommandActionIOType.DirectoryCopy:
					DirectoryHelper.Copy(DestinationPath, SourcePath);
					break;
				case CommandActionIOType.DirectoryMove:
					DirectoryHelper.Move(DestinationPath, SourcePath);
					break;
				case CommandActionIOType.DirectoryCreate:
					DirectoryHelper.DeleteIfExists(DestinationPath);
					break;
				case CommandActionIOType.FileCreate:
					FileHelper.DeleteIfExists(DestinationPath);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}