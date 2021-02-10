using GothicModComposer.Commands.ExecutedCommandActions.Interfaces;
using GothicModComposer.Utils;
using GothicModComposer.Utils.IOHelpers;

namespace GothicModComposer.Commands.ExecutedCommandActions
{
	public class CommandActionIO : ICommandActionIO
	{
		public CommandActionIOType ActionType { get; }
		public string SourcePath { get; }
		public string DestinationPath { get; }

		private CommandActionIO(CommandActionIOType actionType, string sourcePath, string destinationPath)
		{
			ActionType = actionType;
			SourcePath = sourcePath;
			DestinationPath = destinationPath;
		}
		
		public static CommandActionIO FileCreated(string path) 
			=> new CommandActionIO(CommandActionIOType.FileCreate, null, path);
		public static CommandActionIO FileCopied(string sourcePath, string destinationPath) 
			=> new CommandActionIO(CommandActionIOType.FileCopy, sourcePath, destinationPath);
		public static CommandActionIO FileMoved(string sourcePath, string destinationPath)
			=> new CommandActionIO(CommandActionIOType.FileMove, sourcePath, destinationPath);
		public static CommandActionIO DirectoryCreated(string path)
			=> new CommandActionIO(CommandActionIOType.DirectoryCreate, null, path);
		public static CommandActionIO DirectoryCopied(string sourcePath, string destinationPath)
			=> new CommandActionIO(CommandActionIOType.DirectoryCopy, sourcePath, destinationPath);
		public static CommandActionIO DirectoryMoved(string sourcePath, string destinationPath)
			=> new CommandActionIO(CommandActionIOType.DirectoryMove, sourcePath, destinationPath);


		public static CommandActionIO DirectoryDeleted(string path)
		{
			var action = new CommandActionIO(CommandActionIOType.DirectoryDelete, null, path);

			// TODO: For this action we need to copy deleting directory content into some tmp folder due to undo request or dispose at the end of profile processing

			return action;
		}
		
		public static CommandActionIO FileDeleted(string path)
		{
			var action = new CommandActionIO(CommandActionIOType.FileDelete, null, path);

			// TODO: For this action we need to copy deleting file content into some tmp folder due to undo request or dispose at the end of profile processing

			return action;
		}

		public void Undo()
		{
			switch (ActionType)
			{
				case CommandActionIOType.FileCreate:
					FileHelper.DeleteIfExists(DestinationPath);
					break;
				case CommandActionIOType.FileCopy:
					FileHelper.CopyWithOverwrite(DestinationPath, SourcePath);
					break;
				case CommandActionIOType.FileMove:
					FileHelper.MoveWithOverwrite(DestinationPath, SourcePath);
					break;
				case CommandActionIOType.FileDelete:
					Logger.Warn("Deleting file undo action is not implemented yet.");
					break;
				case CommandActionIOType.DirectoryCreate:
					DirectoryHelper.DeleteIfExists(DestinationPath);
					break;
				case CommandActionIOType.DirectoryCopy:
					DirectoryHelper.Copy(DestinationPath, SourcePath);
					break;
				case CommandActionIOType.DirectoryMove:
					DirectoryHelper.Move(DestinationPath, SourcePath);
					break;
				case CommandActionIOType.DirectoryDelete:
					Logger.Warn("Deleting directory undo action is not implemented yet.");
					break;
				default:
					Logger.Warn("Unknown type of a single command action.");
					break;
			}
		}
	}
}