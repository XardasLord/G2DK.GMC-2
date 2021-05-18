﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using GothicModComposer.UI.Commands;
using GothicModComposer.UI.Helpers;
using GothicModComposer.UI.Models;
using Ookii.Dialogs.Wpf;

namespace GothicModComposer.UI.ViewModels
{
    public class GmcSettingsVM : ObservableVM
    {
        private GmcConfiguration _gmcConfiguration;
        private ObservableCollection<string> _zen3DWorlds;

        public string GmcSettingsJsonFilePath { get; }
        public string LogsDirectoryPath => Path.Combine(GmcConfiguration?.Gothic2RootPath ?? string.Empty, ".gmc", "Logs");

        public GmcConfiguration GmcConfiguration
        {
            get => _gmcConfiguration;
            set => SetProperty(ref _gmcConfiguration, value);
        }

        public ObservableCollection<string> Zen3DWorlds
        {
            get => _zen3DWorlds;
            set => SetProperty(ref _zen3DWorlds, value);
        }

        public RelayCommand SelectGothic2RootDirectory { get; }
        public RelayCommand SelectModificationRootDirectory { get; }
        public RelayCommand SaveSettings { get; }
        public RelayCommand RestoreDefaultConfiguration { get; }
        public RelayCommand OpenLogsDirectory { get; }
        public RelayCommand ClearLogsDirectory { get; }

        public GmcSettingsVM()
        {
            GmcSettingsJsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "gmc-2-ui.json");
            Zen3DWorlds = new ObservableCollection<string>();

            if (!File.Exists(GmcSettingsJsonFilePath))
                CreateDefaultConfigurationFile();

            LoadConfiguration();
            
            GmcConfiguration.PropertyChanged += (_, _) => SaveSettings.Execute(null);

            // TODO: Would be nice to have this operation async
            LoadZen3DWorlds();

            SelectGothic2RootDirectory = new RelayCommand(SelectGothic2RootDirectoryExecute);
            SelectModificationRootDirectory = new RelayCommand(SelectModificationRootDirectoryExecute);
            SaveSettings = new RelayCommand(SaveSettingsExecute);
            RestoreDefaultConfiguration = new RelayCommand(RestoreDefaultConfigurationExecute);
            OpenLogsDirectory = new RelayCommand(OpenLogsDirectoryExecute);
            ClearLogsDirectory = new RelayCommand(ClearLogsDirectoryExecute);
        }

        private void SelectGothic2RootDirectoryExecute(object obj)
        {
            var openFolderDialog = new VistaFolderBrowserDialog
            {
                Description = "Select Gothic II root folder",
                UseDescriptionForTitle = true
            };

            openFolderDialog.ShowDialog();

            if (string.IsNullOrWhiteSpace(openFolderDialog.SelectedPath))
                return;
            
            GmcConfiguration.Gothic2RootPath = openFolderDialog.SelectedPath;
            OnPropertyChanged(nameof(GmcConfiguration));
            
            LoadZen3DWorlds();
        }

        private void SelectModificationRootDirectoryExecute(object obj)
        {
            var openFolderDialog = new VistaFolderBrowserDialog
            {
                Description = "Select modification root folder",
                UseDescriptionForTitle = true
            };

            openFolderDialog.ShowDialog();

            if (string.IsNullOrWhiteSpace(openFolderDialog.SelectedPath))
                return;

            GmcConfiguration.ModificationRootPath = openFolderDialog.SelectedPath;
            OnPropertyChanged(nameof(GmcConfiguration));
        }

        private void SaveSettingsExecute(object obj)
        {
            var configurationJson = JsonSerializer.Serialize(GmcConfiguration);
            File.WriteAllText(GmcSettingsJsonFilePath, configurationJson);
        }

        private void RestoreDefaultConfigurationExecute(object obj)
        {
            if (File.Exists(GmcSettingsJsonFilePath))
            {
                File.Delete(GmcSettingsJsonFilePath);
            }
            
            CreateDefaultConfigurationFile();
            LoadConfiguration();
        }
        
        private void OpenLogsDirectoryExecute(object obj)
        {
            if (Directory.Exists(LogsDirectoryPath))
                Process.Start("explorer.exe", LogsDirectoryPath);
        }

        private void ClearLogsDirectoryExecute(object obj)
        {
            if (!Directory.Exists(LogsDirectoryPath))
                return;
            
            var directoryInfo = new DirectoryInfo(LogsDirectoryPath);

            foreach (var file in directoryInfo.GetFiles())
                file.Delete(); 
            
            foreach (var dir in directoryInfo.GetDirectories())
                dir.Delete(true); 
        }

        private void CreateDefaultConfigurationFile()
        {
            var configurationFile = GmcConfiguration.CreateDefault();

            var configurationJson = JsonSerializer.Serialize(configurationFile);

            File.WriteAllText(GmcSettingsJsonFilePath, configurationJson);
        }

        private void LoadConfiguration()
        {
            var configurationJson = File.ReadAllText(GmcSettingsJsonFilePath);

            GmcConfiguration = JsonSerializer.Deserialize<GmcConfiguration>(configurationJson);

            if (GmcConfiguration != null && GmcConfiguration.GothicArguments.Resolution is null)
            {
                GmcConfiguration.GothicArguments.Resolution = new Resolution {Width = 800, Height = 600};
            }
        }

        public void LoadZen3DWorlds()
        {
            Zen3DWorlds.Clear();
            
            if (GmcConfiguration.Gothic2RootPath is null)
                return;
            
            var worldsPath = Path.Combine(GmcConfiguration.Gothic2RootPath, "_Work", "Data", "Worlds");

            if (!Directory.Exists(worldsPath))
                return;

            var worldFiles = Directory.EnumerateFiles(worldsPath, "*.ZEN", SearchOption.AllDirectories).ToList();
            worldFiles.ForEach(zenFilePath =>
            {
                if (HasBinaryContent(zenFilePath))
                    Zen3DWorlds.Add(new FileInfo(zenFilePath).Name);
            });
        }
        
        private static bool HasBinaryContent(string filePath)
        {
            if (!File.Exists(filePath)) 
                return false;
            
            var content = File.ReadAllBytes(filePath);
                
            for (var i = 1; i < 512 && i < content.Length; i++) {
                // Is it binary? Check for consecutive nulls..
                if (content[i] == 0x00 && content[i-1] == 0x00)
                {
                    return true;
                }
            }
                
            return false;
        }
    }
}