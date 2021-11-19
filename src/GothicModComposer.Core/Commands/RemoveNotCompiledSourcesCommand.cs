using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public class RemoveNotCompiledSourcesCommand : ICommand
    {
        private static readonly Stack<ICommandActionIO> ExecutedActions = new();

        private readonly List<AssetPresetType> _assertsToRemoveFilesFrom = new()
        {
            AssetPresetType.Textures,
            AssetPresetType.Meshes
        };

        private readonly IProfile _profile;
        private ProgressBar _parentProgressBar;

        public RemoveNotCompiledSourcesCommand(IProfile profile)
            => _profile = profile;

        public string CommandName => "Remove not compiled sources inside Textures/Meshes assets";

        public async Task ExecuteAsync()
        {
            _parentProgressBar = new ProgressBar(2, "Removing not compiled base files from _Work/Data",
                ProgressBarOptionsHelper.Get());

            AssetPresetFolders.FoldersWithAssets.ForEach(assetType =>
            {
                if (!_assertsToRemoveFilesFrom.Exists(x => x == assetType))
                    return;

                var assetFolderPath = Path.Combine(_profile.GothicFolder.WorkDataFolderPath, assetType.ToString());
                var assetFolder = new AssetFolder(assetFolderPath, assetType);

                DeleteSubFolders(assetFolder);

                _parentProgressBar.Tick();

                DeleteFiles(assetFolder);
            });

            _parentProgressBar.Dispose();
        }

        public void Undo() => ExecutedActions.Undo();

        private void DeleteFiles(AssetFolder assetFolder)
        {
            // TODO: Refactor - introduce something like AssetSubFolder and AssetFile objects and move some of the logic there.

            var filesInAssetFolder =
                DirectoryHelper.GetAllFilesInDirectory(assetFolder.BasePath, SearchOption.TopDirectoryOnly);

            using var childProgressBar = _parentProgressBar.Spawn(filesInAssetFolder.Count,
                "Creating backup and delete not compiled files", ProgressBarOptionsHelper.Get());

            var counter = 1;
            foreach (var assetFile in filesInAssetFolder)
            {
                var tmpCommandActionBackupPath = GetTmpBackupPathForFile(assetFile);

                FileHelper.CopyWithOverwrite(assetFile, tmpCommandActionBackupPath);

                FileHelper.DeleteIfExists(assetFile);
                ExecutedActions.Push(CommandActionIO.FileDeleted(assetFile, tmpCommandActionBackupPath));

                childProgressBar.Tick(
                    $"Created backup and deleted {counter++} of {filesInAssetFolder.Count} files inside '{assetFolder.AssetFolderName}' asset folder");
            }
        }

        private void DeleteSubFolders(AssetFolder assetFolder)
        {
            var subDirectories = assetFolder
                .SubDirectories
                .Where(subDirectoryPath => !Path.GetFileName(subDirectoryPath).Equals("_compiled"))
                .Where(subDirectoryPath => !Path.GetFileName(subDirectoryPath).Equals("Level"));

            using var childProgressBar = _parentProgressBar.Spawn(subDirectories.Count(),
                "Creating backup and delete subfolders", ProgressBarOptionsHelper.Get());

            var counter = 1;
            foreach (var subDirectoryPath in subDirectories)
            {
                var tmpCommandActionBackupPath = GetTmpBackupPathForDirectory(subDirectoryPath);

                if (DirectoryHelper.Exists(tmpCommandActionBackupPath))
                    DirectoryHelper.DeleteIfExists(tmpCommandActionBackupPath);

                DirectoryHelper.Copy(subDirectoryPath, tmpCommandActionBackupPath);

                DirectoryHelper.DeleteIfExists(subDirectoryPath);
                ExecutedActions.Push(CommandActionIO.DirectoryDeleted(subDirectoryPath, tmpCommandActionBackupPath));

                childProgressBar.Tick(
                    $"Created backup and deleted {counter++} of {subDirectories.Count()} subfolders inside '{assetFolder.AssetFolderName}' asset folder");
            }
        }

        private string GetTmpBackupPathForDirectory(string subDirectoryPath)
        {
            var directoryInfo = new DirectoryInfo(subDirectoryPath);
            var twoLastDirectories = Path.Combine(Path.GetFileName(directoryInfo.Parent.FullName), directoryInfo.Name);

            var tmpCommandActionBackupPath =
                Path.Combine(_profile.GmcFolder.GetTemporaryCommandActionBackupPath(GetType().Name),
                    twoLastDirectories);

            return tmpCommandActionBackupPath;
        }

        private string GetTmpBackupPathForFile(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            var parentDirectory = Path.GetFileName(fileInfo.DirectoryName);

            var tmpCommandActionBackupPath =
                Path.Combine(_profile.GmcFolder.GetTemporaryCommandActionBackupPath(GetType().Name), parentDirectory,
                    Path.GetFileName(filePath));

            return tmpCommandActionBackupPath;
        }
    }
}