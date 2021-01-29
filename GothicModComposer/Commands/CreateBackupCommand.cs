using System.IO;
using GothicModComposer.Models;
using GothicModComposer.Presets;
using GothicModComposer.Utils;

namespace GothicModComposer.Commands
{
	public class CreateBackupCommand : ICommand
	{
		public string CommandName => "Create original Gothic backup";

		private readonly GothicFolder _gothicFolder;
		private readonly GmcFolder _gmcFolder;
		private readonly ModFolder _modFolder;

		public CreateBackupCommand(GothicFolder gothicFolder, GmcFolder gmcFolder, ModFolder modFolder)
		{
			_gothicFolder = gothicFolder;
			_gmcFolder = gmcFolder;
			_modFolder = modFolder;
		}

		public void Execute()
		{
			if (_gmcFolder.DoesBackupFolderExist)
			{
				Logger.Info("Backup folder already exists.");
				return;
			}

			BackupGothicWorkDataFolder();
			BackupFilesOverridenByExtensions();
		}

		public void Revert()
		{
		}

		private void BackupGothicWorkDataFolder()
		{
			_gmcFolder.CreateBackupWorkDataFolder();
			AssetPresetFolders.FoldersWithAssets.ForEach(assetFolder =>
			{
				var sourcePath = Path.Combine(_gothicFolder.WorkDataFolderPath, assetFolder.ToString());
				var destinationPath = Path.Combine(_gmcFolder.BackupWorkDataFolderPath, assetFolder.ToString());

				if (!Directory.Exists(sourcePath)) 
					return;
				
				Directory.Move(sourcePath, destinationPath);
				Logger.Info($"Moved directory \"{sourcePath}\" ---> \"{destinationPath}\"");
			});
		}

		private void BackupFilesOverridenByExtensions() 
			=> DirectoryHelper
				.GetAllFilesInDirectory(_modFolder.ExtensionsFolderPath)
				.ForEach(BackupFileFromExtensionFolder);

		private void BackupFileFromExtensionFolder(string filePath)
		{
			var extensionFileRelativePath = DirectoryHelper.ToRelativePath(filePath, _modFolder.ExtensionsFolderPath);
			var extensionFileGothicPath = DirectoryHelper.MergeRelativePath(_gothicFolder.BasePath, extensionFileRelativePath);
			var extensionFileGmcBackupPath = DirectoryHelper.MergeRelativePath(_gmcFolder.BackupFolderPath, extensionFileRelativePath);

			if (!File.Exists(extensionFileGothicPath))
				return;
			
			var folderFromExtensionDirectory = Path.GetDirectoryName(extensionFileGmcBackupPath);
			DirectoryHelper.CreateDirectoryIfDoesNotExist(folderFromExtensionDirectory);
			DirectoryHelper.CopyFileWithOverwrite(extensionFileGothicPath, extensionFileGmcBackupPath);
			
			Logger.Info($"Moved extension file \"{extensionFileGothicPath}\" ---> \"{extensionFileGmcBackupPath}\"");
		}
	}
}