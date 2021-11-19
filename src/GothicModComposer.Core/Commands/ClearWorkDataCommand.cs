using System.Collections.Generic;
using System.IO;
using GothicModComposer.Core.Commands.ExecutedCommandActions;
using GothicModComposer.Core.Commands.ExecutedCommandActions.Interfaces;
using GothicModComposer.Core.Models.Folders;
using GothicModComposer.Core.Models.Profiles;
using GothicModComposer.Core.Presets;
using GothicModComposer.Core.Utils.IOHelpers;
using GothicModComposer.Core.Utils.ProgressBar;
using ShellProgressBar;

namespace GothicModComposer.Core.Commands
{
    public class ClearWorkDataCommand : ICommand
    {
        private static readonly Stack<ICommandActionIO> ExecutedActions = new();

        private readonly IProfile _profile;

        public ClearWorkDataCommand(IProfile profile)
            => _profile = profile;

        public string CommandName => "Clear Gothic '_Work/Data' folder";

        public async Task ExecuteAsync()
        {
            using (var progress = new ProgressBar(AssetPresetFolders.FoldersWithAssets.Count,
                "Clearing _Work/Data folder", ProgressBarOptionsHelper.Get()))
            {
                AssetPresetFolders.FoldersWithAssets.ForEach(assetType =>
                {
                    var assetFolderPath = Path.Combine(_profile.GothicFolder.WorkDataFolderPath, assetType.ToString());

                    var assetFolder = new AssetFolder(assetFolderPath, assetType);
                    if (assetFolder.Exists())
                    {
                        var tmpCommandActionBackupPath = Path.Combine(
                            _profile.GmcFolder.GetTemporaryCommandActionBackupPath(GetType().Name),
                            Path.GetFileName(assetFolder.BasePath));

                        DirectoryHelper.Copy(assetFolder.BasePath, tmpCommandActionBackupPath);
                        ExecutedActions.Push(CommandActionIO.DirectoryDeleted(assetFolder.BasePath,
                            tmpCommandActionBackupPath));

                        assetFolder.Delete();
                    }

                    // TODO: Should it be the scope of this command? I guess not.
                    if (assetFolder.IsCompilable())
                    {
                        assetFolder.CreateCompiledFolder();
                        ExecutedActions.Push(CommandActionIO.DirectoryCreated(assetFolder.CompiledFolderPath));
                    }

                    progress.Tick();
                });
            }

            var modFileTrackerPath = _profile.GmcFolder.ModFilesTrackerFilePath;
            if (FileHelper.Exists(modFileTrackerPath))
            {
                var tmpCommandActionBackupPath =
                    Path.Combine(_profile.GmcFolder.GetTemporaryCommandActionBackupPath(GetType().Name),
                        Path.GetFileName(modFileTrackerPath));

                FileHelper.CopyWithOverwrite(modFileTrackerPath, tmpCommandActionBackupPath);

                _profile.GmcFolder.DeleteTrackerFileIfExist();

                ExecutedActions.Push(CommandActionIO.FileDeleted(_profile.GmcFolder.ModFilesTrackerFilePath,
                    tmpCommandActionBackupPath));
            }
        }

        public void Undo() => ExecutedActions.Undo();
    }
}