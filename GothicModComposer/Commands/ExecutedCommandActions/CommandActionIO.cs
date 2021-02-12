﻿using GothicModComposer.Commands.ExecutedCommandActions.Interfaces;
using GothicModComposer.Utils;
using GothicModComposer.Utils.IOHelpers;

namespace GothicModComposer.Commands.ExecutedCommandActions
{
	public class CommandActionIO : ICommandActionIO
	{
		public CommandActionIOType ActionType { get; }
		public string SourcePath { get; }
		public string DestinationPath { get; }
		public string OriginalFilePathBackup { get; }

		private CommandActionIO(CommandActionIOType actionType, string sourcePath, string destinationPath, string originalFilePathBackup = null)
		{
			ActionType = actionType;
			SourcePath = sourcePath;
			DestinationPath = destinationPath;
			OriginalFilePathBackup = originalFilePathBackup;
		}
		
		public static CommandActionIO FileCreated(string path) 
			=> new CommandActionIO(CommandActionIOType.FileCreate, null, path);
		public static CommandActionIO FileCopied(string sourcePath, string destinationPath) 
			=> new CommandActionIO(CommandActionIOType.FileCopy, sourcePath, destinationPath);
		public static CommandActionIO FileCopiedWithOverwrite(string destinationPath, string originalFilePathBackup)
			=> new CommandActionIO(CommandActionIOType.FileCopyWithOverwrite, null, destinationPath, originalFilePathBackup);
		public static CommandActionIO FileMoved(string sourcePath, string destinationPath)
			=> new CommandActionIO(CommandActionIOType.FileMove, sourcePath, destinationPath);
		public static CommandActionIO FileDeleted(string path, string originalFilePathBackup)
			=> new CommandActionIO(CommandActionIOType.FileDelete, null, path, originalFilePathBackup);
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

		public void Undo()
		{
			switch (ActionType)
			{
				case CommandActionIOType.FileCreate:
					FileHelper.DeleteIfExists(DestinationPath);
					break;
				case CommandActionIOType.FileCopy:
					FileHelper.DeleteIfExists(DestinationPath);
					break;
				case CommandActionIOType.FileCopyWithOverwrite:
					FileHelper.CopyWithOverwrite(OriginalFilePathBackup, DestinationPath);
					break;
				case CommandActionIOType.FileMove:
					FileHelper.MoveWithOverwrite(DestinationPath, SourcePath);
					break;
				case CommandActionIOType.FileDelete:
					FileHelper.CopyWithOverwrite(OriginalFilePathBackup, DestinationPath);
					break;
				case CommandActionIOType.DirectoryCreate:
					DirectoryHelper.DeleteIfExists(DestinationPath);
					break;
				case CommandActionIOType.DirectoryCopy:
					// TODO: Directory should be deleted from DestinationPath instead of copying back. We also should split th copying into two parts (copy and copy with overwrite) like with files
					DirectoryHelper.Copy(DestinationPath, SourcePath);
					break;
				case CommandActionIOType.DirectoryMove:
					DirectoryHelper.DeleteIfExists(SourcePath);
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