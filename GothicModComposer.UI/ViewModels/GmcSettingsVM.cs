using System;
using System.Collections.ObjectModel;
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
        private bool _changesMade;

        public string GmcSettingsJsonFilePath { get; }

        public GmcConfiguration GmcConfiguration
        {
            get => _gmcConfiguration;
            set
            {
                if (SetProperty(ref _gmcConfiguration, value))
                    ChangesMade = true;
            }
        }

        public ObservableCollection<string> Zen3DWorlds
        {
            get => _zen3DWorlds;
            set
            {
                if (SetProperty(ref _zen3DWorlds, value))
                    ChangesMade = true;
            }
        }

        public bool ChangesMade
        {
            get => _changesMade;
            set => SetProperty(ref _changesMade, value);
        }

        public RelayCommand SelectGothic2RootDirectory { get; }
        public RelayCommand SelectModificationRootDirectory { get; }
        public RelayCommand SaveSettings { get; }

        public GmcSettingsVM()
        {
            GmcSettingsJsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "gmc-2-ui.json");
            Zen3DWorlds = new ObservableCollection<string>();

            if (!File.Exists(GmcSettingsJsonFilePath))
                CreateDefaultConfigurationFile();

            LoadConfiguration();
            GmcConfiguration.PropertyChanged += (_, _) => ChangesMade = true;

            // TODO: Would be nice to have this operation async
            LoadZen3DWorlds();

            SelectGothic2RootDirectory = new RelayCommand(SelectGothic2RootDirectoryExecute);
            SelectModificationRootDirectory = new RelayCommand(SelectModificationRootDirectoryExecute);
            SaveSettings = new RelayCommand(SaveSettingsExecute);
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
            
            LoadZen3DWorlds();
        }

        private void SaveSettingsExecute(object obj)
        {
            var configurationJson = JsonSerializer.Serialize(GmcConfiguration);
            File.WriteAllText(GmcSettingsJsonFilePath, configurationJson);

            ChangesMade = false;
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
        }

        private void LoadZen3DWorlds()
        {
            Zen3DWorlds.Clear();
            
            if (GmcConfiguration.ModificationRootPath is null)
                return;
            
            var worldsPath = Path.Combine(GmcConfiguration.ModificationRootPath, "Worlds");

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