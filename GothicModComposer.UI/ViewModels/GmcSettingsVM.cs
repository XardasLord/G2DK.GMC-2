using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Windows;
using GothicModComposer.UI.Commands;
using GothicModComposer.UI.Helpers;
using GothicModComposer.UI.Models;
using Ookii.Dialogs.Wpf;

namespace GothicModComposer.UI.ViewModels
{
    public class GmcSettingsVM : ObservableVM
    {
        private GmcConfiguration _gmcConfiguration;
        private ObservableCollection<Zen3DWorld> _zen3DWorlds;
        private bool _isSystemPackAvailable;
        private readonly FileSystemWatcher _zenWorldsFileWatcher;

        public string GmcSettingsJsonFilePath { get; }
        public string LogsDirectoryPath => Path.Combine(GmcConfiguration?.Gothic2RootPath ?? string.Empty, ".gmc", "Logs");

        public GmcConfiguration GmcConfiguration
        {
            get => _gmcConfiguration;
            set => SetProperty(ref _gmcConfiguration, value);
        }

        public ObservableCollection<Zen3DWorld> Zen3DWorlds
        {
            get => _zen3DWorlds;
            set => SetProperty(ref _zen3DWorlds, value);
        }

        public bool IsSystemPackAvailable
        {
            get => _isSystemPackAvailable;
            set => SetProperty(ref _isSystemPackAvailable, value);
        }

        public RelayCommand SelectGothic2RootDirectory { get; }
        public RelayCommand SelectModificationRootDirectory { get; }
        public RelayCommand SaveSettings { get; }
        public RelayCommand RestoreDefaultConfiguration { get; }
        public RelayCommand OpenLogsDirectory { get; }
        public RelayCommand ClearLogsDirectory { get; }
        public RelayCommand RestoreDefaultIniOverrides { get; }

        public GmcSettingsVM()
        {
            _zenWorldsFileWatcher = new FileSystemWatcher();
            
            GmcSettingsJsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "gmc-2-ui.json");
            Zen3DWorlds = new ObservableCollection<Zen3DWorld>();

            SelectGothic2RootDirectory = new RelayCommand(SelectGothic2RootDirectoryExecute);
            SelectModificationRootDirectory = new RelayCommand(SelectModificationRootDirectoryExecute);
            SaveSettings = new RelayCommand(SaveSettingsExecute);
            RestoreDefaultConfiguration = new RelayCommand(RestoreDefaultConfigurationExecute);
            OpenLogsDirectory = new RelayCommand(OpenLogsDirectoryExecute);
            ClearLogsDirectory = new RelayCommand(ClearLogsDirectoryExecute);
            RestoreDefaultIniOverrides = new RelayCommand(RestoreDefaultIniOverridesExecute);

            if (!File.Exists(GmcSettingsJsonFilePath))
                CreateDefaultConfigurationFile();

            LoadConfiguration();
            
            GmcConfiguration.PropertyChanged += (_, _) => SaveSettings.Execute(null);
            GmcConfiguration.IniOverrides.CollectionChanged += IniOverrides_CollectionChanged;
            GmcConfiguration.IniOverridesSystemPack.CollectionChanged += IniOverrides_CollectionChanged;
            
            GmcConfiguration.OnGothic2RootPathChanged += OnGothic2RootPathChanged;
            
            if (!string.IsNullOrWhiteSpace(GmcConfiguration.Gothic2RootPath))
                OnGothic2RootPathChanged(GmcConfiguration.Gothic2RootPath);

            // TODO: Would be nice to have this operation async
            LoadZen3DWorlds();
        }

        private void IniOverrides_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (INotifyPropertyChanged added in e.NewItems)
                {
                    added.PropertyChanged += (_, _) => SaveSettings.Execute(null);
                }

            if (e.OldItems != null)
                foreach (INotifyPropertyChanged removed in e.OldItems)
                {
                    removed.PropertyChanged -= (_, _) => SaveSettings.Execute(null);
                }

            SaveSettings.Execute(null);
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

            IsSystemPackAvailable = File.Exists(Path.Combine(GmcConfiguration.Gothic2RootPath, "System", "SystemPack.ini"));
            
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
            var configurationJson = JsonSerializer.Serialize(GmcConfiguration, new JsonSerializerOptions
            {
                WriteIndented = true
            });

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

            try
            {
	            foreach (var file in directoryInfo.GetFiles())
		            file.Delete();

	            foreach (var dir in directoryInfo.GetDirectories())
		            dir.Delete(true);
            }
            catch (IOException ex)
            {
	            MessageBox.Show($"Cannot clear Logs directory. Try to run GMC application again with Administrator privileges.{Environment.NewLine}Reason: {ex.Message}",
		            "Cannot clear Logs directory", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RestoreDefaultIniOverridesExecute(object obj)
        {
            GmcConfiguration.IniOverrides.Clear();
            
            AddMissingDefaultIniOverrides();
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

            try
            {
                GmcConfiguration = JsonSerializer.Deserialize<GmcConfiguration>(configurationJson);
            }
            catch
            {
                MessageBox.Show(
                    $"Your gmc-2-ui.json file under {GmcSettingsJsonFilePath} path has invalid format.{Environment.NewLine}{Environment.NewLine}Please delete this file and run GMC UI again.",
                    "Invalid configuration file format", 
                    MessageBoxButton.OK, MessageBoxImage.Error);

                Environment.Exit(0);
            }

            if (GmcConfiguration != null && GmcConfiguration.GothicArguments.Resolution is null)
            {
                GmcConfiguration.GothicArguments.Resolution = new Resolution {Width = 800, Height = 600};
            }
            
            AddMissingDefaultIniOverrides();
            RemoveExistingIniOverridesThatAreNotDefaults();

            foreach (var iniOverrideItem in GmcConfiguration.IniOverrides)
            {
                iniOverrideItem.PropertyChanged += (_, _) => SaveSettings.Execute(null);
            }

            foreach (var iniOverrideItem in GmcConfiguration.IniOverridesSystemPack)
            {
                iniOverrideItem.PropertyChanged += (_, _) => SaveSettings.Execute(null);
            }

            IsSystemPackAvailable = File.Exists(Path.Combine(GmcConfiguration.Gothic2RootPath, "System", "SystemPack.ini"));
        }

        private void AddMissingDefaultIniOverrides()
        {
            var defaultIniOverrideAdded = false;

            IniOverrideHelper.DefaultIniOverrideKeys.ForEach(defaultIniOverrideItem =>
            {
                if (GmcConfiguration.IniOverrides.Any(x => x.Key == defaultIniOverrideItem.Key))
                {
                    // Just to be sure if section is not filled
                    GmcConfiguration.IniOverrides.Single(x => x.Key == defaultIniOverrideItem.Key).Section = defaultIniOverrideItem.Section;
                    GmcConfiguration.IniOverrides.Single(x => x.Key == defaultIniOverrideItem.Key).DisplayAs = defaultIniOverrideItem.DisplayAs;
                    GmcConfiguration.IniOverrides.Single(x => x.Key == defaultIniOverrideItem.Key).AvailableValues = defaultIniOverrideItem.AvailableValues;
                    return;
                }

                GmcConfiguration.IniOverrides.Add(defaultIniOverrideItem);
                defaultIniOverrideAdded = true;
            });

            if (defaultIniOverrideAdded)
                SaveSettings.Execute(null);
        }

        private void RemoveExistingIniOverridesThatAreNotDefaults()
        {
            var iniOverrideRemoved = false;

            foreach (var iniOverrideItemFromConfiguration in GmcConfiguration.IniOverrides.Reverse())
            {
                if (IniOverrideHelper.DefaultIniOverrideKeys.Any(x => x.Key == iniOverrideItemFromConfiguration.Key))
                    continue;

                GmcConfiguration.IniOverrides.Remove(iniOverrideItemFromConfiguration);
                iniOverrideRemoved = true;
            }

            if (iniOverrideRemoved)
                SaveSettings.Execute(null);
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
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (!HasBinaryContent(zenFilePath))
                        return;

                    var zenFileInfo = new FileInfo(zenFilePath);

                    Zen3DWorlds.Add(new Zen3DWorld(zenFilePath, zenFileInfo.Name));
                });
            });

            foreach (var zen3DWorld in Zen3DWorlds)
            {
                zen3DWorld.SetAsUnselected();
                
                if (zen3DWorld.Path == GmcConfiguration.DefaultWorld)
                    zen3DWorld.SetAsSelected();
            }

            if (Zen3DWorlds.All(x => !x.IsSelected))
                GmcConfiguration.DefaultWorld = null;
        }

        private bool HasBinaryContent(string filePath)
        {
            var fileInfo = new FileInfo(filePath);

            if (IsFileLocked(fileInfo))
            {
                return false;
            }
            
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
        
        private void OnGothic2RootPathChanged(string gothic2RootPath)
        {
            var worldsPath = Path.Combine(GmcConfiguration.Gothic2RootPath, "_Work", "Data", "Worlds");

            if (!Directory.Exists(worldsPath))
            {
                UnsubscribeOnWorldDirectoryChanges();
            }
            
            _zenWorldsFileWatcher.Path = gothic2RootPath;
            _zenWorldsFileWatcher.IncludeSubdirectories = true;
            
            SubscribeOnWorldDirectoryChanges();
        }

        public void SubscribeOnWorldDirectoryChanges()
        {
            _zenWorldsFileWatcher.Created += ZenWorldFilesChanged;
            _zenWorldsFileWatcher.Renamed += ZenWorldFilesChanged;
            _zenWorldsFileWatcher.Deleted += ZenWorldFilesChanged;
            
            _zenWorldsFileWatcher.EnableRaisingEvents = true;
            
            // Force to LoadZen3DWorlds when starting subscribing
            Application.Current.Dispatcher.Invoke(LoadZen3DWorlds);
        }

        public void UnsubscribeOnWorldDirectoryChanges()
        {
            _zenWorldsFileWatcher.Created -= ZenWorldFilesChanged;
            _zenWorldsFileWatcher.Renamed -= ZenWorldFilesChanged;
            _zenWorldsFileWatcher.Deleted -= ZenWorldFilesChanged;
            
            _zenWorldsFileWatcher.EnableRaisingEvents = false;
        }

        private void ZenWorldFilesChanged(object sender, FileSystemEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(LoadZen3DWorlds);
        }
        
        private bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }
    }
}