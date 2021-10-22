﻿using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using GothicModComposer.Commands.ExecutedCommandActions;
using GothicModComposer.Commands.ExecutedCommandActions.Interfaces;
using GothicModComposer.Models.Profiles;
using GothicModComposer.Utils;
using GothicModComposer.Utils.IOHelpers;
using GothicModComposer.Utils.ProgressBar;
using ShellProgressBar;

namespace GothicModComposer.Commands
{
    public class UpdateModExtensionFilesCommand : ICommand
    {
        public string CommandName => "Update mod extension files";

        private readonly IProfile _profile;
        private static readonly ConcurrentStack<ICommandActionIO> ExecutedActions = new();
        private static object _lock = new();

        public UpdateModExtensionFilesCommand(IProfile profile)
            => _profile = profile;

        public void Execute()
        {
            Logger.Info("Start copying all mod extension files.", true);

            var extensionFiles = DirectoryHelper.GetAllFilesInDirectory(_profile.ModFolder.ExtensionsFolderPath);

            using (var progress = new ProgressBar(extensionFiles.Count, "Updating mod extension files", ProgressBarOptionsHelper.Get()))
            {
                var counter = 1;

                Parallel.ForEach(extensionFiles, extensionFilePathToCopy =>
                {
                    var extensionRelativePath = DirectoryHelper.ToRelativePath(extensionFilePathToCopy, _profile.ModFolder.ExtensionsFolderPath);
                    var extensionDestinationPath = DirectoryHelper.MergeRelativePath(_profile.GothicFolder.BasePath, extensionRelativePath);

                    if (FileHelper.Exists(extensionDestinationPath))
                    {
                        if (FileHelper.GetFileTimestamp(extensionDestinationPath) == FileHelper.GetFileTimestamp(extensionFilePathToCopy))
                        {
                            lock (_lock)
                            {
                                progress.Tick();
                            }

                            return;
                        }

                        var tmpCommandActionBackupPath =
                            Path.Combine(_profile.GmcFolder.GetTemporaryCommandActionBackupPath(GetType().Name), Path.GetFileName(extensionDestinationPath));

                        FileHelper.CopyWithOverwrite(extensionDestinationPath, tmpCommandActionBackupPath);
                        FileHelper.CopyWithOverwrite(extensionFilePathToCopy, extensionDestinationPath);

                        ExecutedActions.Push(CommandActionIO.FileCopiedWithOverwrite(extensionDestinationPath, tmpCommandActionBackupPath));
                    }
                    else
                    {
                        FileHelper.Copy(extensionFilePathToCopy, extensionDestinationPath);
                        ExecutedActions.Push(CommandActionIO.FileCopied(extensionFilePathToCopy, extensionDestinationPath));
                    }

                    lock (_lock)
                    {
                        progress.Tick($"Copied {counter++} of {extensionFiles.Count} files");
                    }
                });
            }
        }

        public void Undo() => ExecutedActions.Undo();
    }
}