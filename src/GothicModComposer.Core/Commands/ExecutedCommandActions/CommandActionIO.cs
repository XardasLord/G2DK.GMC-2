using GothicModComposer.Core.Commands.ExecutedCommandActions.Interfaces;
using GothicModComposer.Core.Utils;
using GothicModComposer.Core.Utils.IOHelpers;

namespace GothicModComposer.Core.Commands.ExecutedCommandActions
{
    public class CommandActionIO : ICommandActionIO
    {
        private CommandActionIO(CommandActionIOType actionType, string sourcePath, string destinationPath,
            string originalFilePathBackup = null)
        {
            ActionType = actionType;
            SourcePath = sourcePath;
            DestinationPath = destinationPath;
            OriginalFilePathBackup = originalFilePathBackup;
        }

        public string OriginalFilePathBackup { get; }
        public CommandActionIOType ActionType { get; }
        public string SourcePath { get; }
        public string DestinationPath { get; }

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
                    DirectoryHelper.DeleteIfExists(DestinationPath);
                    DirectoryHelper.Copy(OriginalFilePathBackup, DestinationPath);
                    break;
                default:
                    Logger.Warn("Unknown type of a single command action.");
                    break;
            }
        }

        public static CommandActionIO FileCreated(string path)
            => new(CommandActionIOType.FileCreate, null, path);

        public static CommandActionIO FileCopied(string sourcePath, string destinationPath)
            => new(CommandActionIOType.FileCopy, sourcePath, destinationPath);

        public static CommandActionIO FileCopiedWithOverwrite(string destinationPath, string originalFilePathBackup)
            => new(CommandActionIOType.FileCopyWithOverwrite, null, destinationPath, originalFilePathBackup);

        public static CommandActionIO FileMoved(string sourcePath, string destinationPath)
            => new(CommandActionIOType.FileMove, sourcePath, destinationPath);

        public static CommandActionIO FileDeleted(string path, string originalFilePathBackup)
            => new(CommandActionIOType.FileDelete, null, path, originalFilePathBackup);

        public static CommandActionIO DirectoryCreated(string path)
            => new(CommandActionIOType.DirectoryCreate, null, path);

        public static CommandActionIO DirectoryCopied(string sourcePath, string destinationPath)
            => new(CommandActionIOType.DirectoryCopy, sourcePath, destinationPath);

        public static CommandActionIO DirectoryMoved(string sourcePath, string destinationPath)
            => new(CommandActionIOType.DirectoryMove, sourcePath, destinationPath);

        public static CommandActionIO DirectoryDeleted(string path, string originalFilePathBackup)
            => new(CommandActionIOType.DirectoryDelete, null, path, originalFilePathBackup);
    }
}