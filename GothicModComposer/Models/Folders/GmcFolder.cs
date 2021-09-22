using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using GothicModComposer.Models.Interfaces;
using GothicModComposer.Models.ModFiles;
using GothicModComposer.Presets;
using GothicModComposer.Utils.IOHelpers;

namespace GothicModComposer.Models.Folders
{
    public class GmcFolder : IGmcFolder
    {
        private readonly IFileSystem _fileSystem;

        private readonly string _tmpCommandActionsBackupFolderPath;

        private GmcFolder(string gmcFolderPath)
        {
            BasePath = gmcFolderPath;
            ModFilesFromTrackedFile = GetModFilesFromTrackerFile();
            _tmpCommandActionsBackupFolderPath = Path.Combine(BasePath, "tmpCommandActionsBackup");
            _fileSystem = new FileSystem();
        }

        public string BasePath { get; }
        public string BackupFolderPath => Path.Combine(BasePath, "backup");
        public string LogsFolderPath => Path.Combine(BasePath, "Logs");
        public string BuildFolderPath => Path.Combine(BasePath, "Build");
        public string VdsfConfigFilePath => Path.Combine(BasePath, "vdfsConfig");
        public string ModFilesTrackerFilePath => Path.Combine(BasePath, "modFiles.json");
        public string BackupWorkDataFolderPath => Path.Combine(BackupFolderPath, "_Work", "Data");
        public bool DoesBackupFolderExist => _fileSystem.Directory.Exists(BackupFolderPath);

        public IEnumerable<string> EssentialDirectoriesFiles => new List<string>
        {
            nameof(AssetPresetType.Presets),
            nameof(AssetPresetType.Music),
            nameof(AssetPresetType.Video),
            $"{nameof(AssetPresetType.Scripts)}\\_compiled"
        };

        public List<ModFileEntry> ModFilesFromTrackedFile { get; }

        public bool CreateGmcFolder() => DirectoryHelper.CreateIfDoesNotExist(BasePath);

        public void CreateBackupWorkDataFolder() => DirectoryHelper.CreateIfDoesNotExist(BackupWorkDataFolderPath);

        public string GetTemporaryCommandActionBackupPath(string commandName)
            => Path.Combine(_tmpCommandActionsBackupFolderPath, commandName);

        public void DeleteTemporaryFiles() => DirectoryHelper.DeleteIfExists(_tmpCommandActionsBackupFolderPath);

        public void AddNewModFileEntryToTrackerFile(ModFileEntry modFileEntry)
        {
            modFileEntry.Timestamp = FileHelper.GetFileTimestamp(modFileEntry.FilePath);
            ModFilesFromTrackedFile.Add(modFileEntry);
        }

        public void UpdateModFileEntryInTrackerFile(ModFileEntry modFileEntry)
        {
            var timestamp = FileHelper.GetFileTimestamp(modFileEntry.FilePath);
            ModFilesFromTrackedFile.Single(x => x.FilePath == modFileEntry.FilePath).Timestamp = timestamp;
        }

        public void RemoveModFileEntryFromTrackerFile(ModFileEntry modFileEntry)
        {
            var index = ModFilesFromTrackedFile.FindIndex(x => x.FilePath == modFileEntry.FilePath);
            ModFilesFromTrackedFile.RemoveAt(index);
        }

        public void SaveTrackerFile()
            => FileHelper.SaveContent(ModFilesTrackerFilePath, JsonSerializer.Serialize(ModFilesFromTrackedFile),
                Encoding.Default);

        public bool DeleteTrackerFileIfExist() => FileHelper.DeleteIfExists(ModFilesTrackerFilePath);

        public static GmcFolder CreateFromPath(string gmcFolderPath)
        {
            var instance = new GmcFolder(gmcFolderPath);

            instance.Verify();

            return instance;
        }

        private List<ModFileEntry> GetModFilesFromTrackerFile()
        {
            if (!FileHelper.Exists(ModFilesTrackerFilePath))
                return new List<ModFileEntry>();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            options.Converters.Add(new JsonStringEnumConverter());

            return JsonSerializer.Deserialize<List<ModFileEntry>>(FileHelper.ReadFile(ModFilesTrackerFilePath),
                options);
        }

        private void Verify()
        {
            // TODO: Verify if the folder exists and is correct
        }
    }
}