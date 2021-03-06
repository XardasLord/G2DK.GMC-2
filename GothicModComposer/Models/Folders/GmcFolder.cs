using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using GothicModComposer.Models.Interfaces;
using GothicModComposer.Models.ModFiles;
using GothicModComposer.Utils.IOHelpers;

namespace GothicModComposer.Models.Folders
{
    public class GmcFolder : IGmcFolder
    {
		public string BasePath { get; }
		public string BackupFolderPath => Path.Combine(BasePath, "backup");
		public string LogsFolderPath => Path.Combine(BasePath, "Logs");
		public string BuildFolderPath => Path.Combine(BasePath, "Build");
		public string VdsfConfigFilePath => Path.Combine(BasePath, "vdfsConfig");
		public string ModFilesTrackerFilePath => Path.Combine(BasePath, "modFiles.json");
		public string BackupWorkDataFolderPath => Path.Combine(BackupFolderPath, "_Work", "Data");
		public bool DoesBackupFolderExist => _fileSystem.Directory.Exists(BackupFolderPath);
		public string EssentialFilesRegexPattern => @"((Presets|Music|Video))|[\/\\](Fonts|_intern)";
		public List<ModFileEntry> ModFilesFromTrackedFile => _modFilesFromTrackerFile;

		private readonly List<ModFileEntry> _modFilesFromTrackerFile;
		private readonly string _tmpCommandActionsBackupFolderPath;

        private readonly IFileSystem _fileSystem;

		private GmcFolder(string gmcFolderPath)
		{
			BasePath = gmcFolderPath;
			_modFilesFromTrackerFile = GetModFilesFromTrackerFile();
			_tmpCommandActionsBackupFolderPath = Path.Combine(BasePath, "tmpCommandActionsBackup");
            _fileSystem = new FileSystem();
        }

		public static GmcFolder CreateFromPath(string gmcFolderPath)
		{
			var instance = new GmcFolder(gmcFolderPath);

			instance.Verify();

			return instance;
		}

		public bool CreateGmcFolder() => DirectoryHelper.CreateIfDoesNotExist(BasePath);

		public void CreateBackupWorkDataFolder() => DirectoryHelper.CreateIfDoesNotExist(BackupWorkDataFolderPath);

		public string GetTemporaryCommandActionBackupPath(string commandName)
			=> Path.Combine(_tmpCommandActionsBackupFolderPath, commandName);

		public void DeleteTemporaryFiles() => DirectoryHelper.DeleteIfExists(_tmpCommandActionsBackupFolderPath);

		public void AddNewModFileEntryToTrackerFile(ModFileEntry modFileEntry)
		{
			modFileEntry.Timestamp = FileHelper.GetFileTimestamp(modFileEntry.FilePath);
			_modFilesFromTrackerFile.Add(modFileEntry);
		}

		public void UpdateModFileEntryInTrackerFile(ModFileEntry modFileEntry)
		{
			var timestamp = FileHelper.GetFileTimestamp(modFileEntry.FilePath);
			_modFilesFromTrackerFile.Single(x => x.FilePath == modFileEntry.FilePath).Timestamp = timestamp;
		}

        public void RemoveModFileEntryFromTrackerFile(ModFileEntry modFileEntry)
        {
            var index = _modFilesFromTrackerFile.FindIndex(x => x.FilePath == modFileEntry.FilePath);
            _modFilesFromTrackerFile.RemoveAt(index);
        }

		public void SaveTrackerFile()
			=> FileHelper.SaveContent(ModFilesTrackerFilePath, JsonSerializer.Serialize(_modFilesFromTrackerFile), Encoding.Default);

		public bool DeleteTrackerFileIfExist() => FileHelper.DeleteIfExists(ModFilesTrackerFilePath);

		private List<ModFileEntry> GetModFilesFromTrackerFile()
		{
			if (!FileHelper.Exists(ModFilesTrackerFilePath))
				return new List<ModFileEntry>();

			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			};
			options.Converters.Add(new JsonStringEnumConverter());

			return JsonSerializer.Deserialize<List<ModFileEntry>>(FileHelper.ReadFile(ModFilesTrackerFilePath), options);
		}

		private void Verify()
		{
			// TODO: Verify if the folder exists and is correct
		}
	}
}