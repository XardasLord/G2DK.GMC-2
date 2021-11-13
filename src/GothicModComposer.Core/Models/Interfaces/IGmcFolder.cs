using System.Collections.Generic;
using GothicModComposer.Core.Models.ModFiles;

namespace GothicModComposer.Core.Models.Interfaces
{
    public interface IGmcFolder
    {
        string BasePath { get; }
        string BackupFolderPath { get; }
        string LogsFolderPath { get; }
        string BuildFolderPath { get; }
        string VdsfConfigFilePath { get; }
        string ModFilesTrackerFilePath { get; }
        string BackupWorkDataFolderPath { get; }
        bool DoesBackupFolderExist { get; }
        IEnumerable<string> EssentialDirectoriesFiles { get; }
        List<ModFileEntry> ModFilesFromTrackedFile { get; }
        bool CreateGmcFolder();
        void CreateBackupWorkDataFolder();
        string GetTemporaryCommandActionBackupPath(string commandName);
        void DeleteTemporaryFiles();
        void AddNewModFileEntryToTrackerFile(ModFileEntry modFileEntry);
        void UpdateModFileEntryInTrackerFile(ModFileEntry modFileEntry);
        void RemoveModFileEntryFromTrackerFile(ModFileEntry modFileEntry);
        void SaveTrackerFile();
        bool DeleteTrackerFileIfExist();
    }
}