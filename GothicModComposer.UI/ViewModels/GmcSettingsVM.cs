using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using GothicModComposer.UI.Commands;
using GothicModComposer.UI.Extensions;
using GothicModComposer.UI.Helpers;
using GothicModComposer.UI.Interfaces;
using GothicModComposer.UI.Models;
using Ookii.Dialogs.Wpf;

namespace GothicModComposer.UI.ViewModels
{
    public class GmcSettingsVM : ObservableVM
    {
        private readonly IFileService _fileService;
        private readonly IGmcDirectoryService _gmcDirectoryService;
        private readonly IZenWorldsFileWatcherService _zenWorldsFileWatcherService;

        private GmcConfiguration _gmcConfiguration;
        private ObservableCollection<Zen3DWorld> _zen3DWorlds;
        private ObservableCollection<Submod> _submods;
        private int _zen3DWorldsLoadingProgress;
        private bool _isSystemPackAvailable;
        private bool _isLogDirectoryAvailable;

        public string GmcSettingsJsonFilePath { get; }

        public string LogsDirectoryPath =>
            Path.Combine(GmcConfiguration?.Gothic2RootPath ?? string.Empty, ".gmc", "Logs");

        public string ModBuildDirectoryPath =>
            Path.Combine(GmcConfiguration?.Gothic2RootPath ?? string.Empty, ".gmc", "build");

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

        public ObservableCollection<Submod> Submods
        {
            get => _submods;
            set => SetProperty(ref _submods, value);
        }

        public int Zen3DWorldsLoadingProgress
        {
            get => _zen3DWorldsLoadingProgress;
            set
            {
                if (value < 0) value = 0;
                if (value > 100) value = 100;
                SetProperty(ref _zen3DWorldsLoadingProgress, value);
            }
        }

        public bool IsSystemPackAvailable
        {
            get => _isSystemPackAvailable;
            set => SetProperty(ref _isSystemPackAvailable, value);
        }
        
        public bool IsLogDirectoryAvailable
        {
            get => _isLogDirectoryAvailable;
            set => SetProperty(ref _isLogDirectoryAvailable, value);
        }
        
        public RelayCommand SelectGothic2RootDirectory { get; }
        public RelayCommand SelectModificationRootDirectory { get; }
        public RelayCommand SaveSettings { get; }
        public RelayCommand RestoreDefaultConfiguration { get; }
        public RelayCommand OpenLogsDirectory { get; }
        public RelayCommand ClearLogsDirectory { get; }
        public RelayCommand OpenModBuildDirectory { get; }
        public RelayCommand RestoreDefaultIniOverrides { get; }
        public RelayCommand OpenGameDirectory { get; }
        public RelayCommand OpenModDirectory { get; }
        
        public GmcSettingsVM(
            IFileService fileService,
            IGmcDirectoryService gmcDirectoryService,
            IZenWorldsFileWatcherService zenWorldsFileWatcherService)
        {
            _fileService = fileService;
            _gmcDirectoryService = gmcDirectoryService;
            _zenWorldsFileWatcherService = zenWorldsFileWatcherService;

            _zenWorldsFileWatcherService.SetHandlers(ZenWorldFilesChanged);

            GmcSettingsJsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "gmc-2-ui.json");
            Zen3DWorlds = new ObservableCollection<Zen3DWorld>();
            Submods = new ObservableCollection<Submod>();
            SelectGothic2RootDirectory = new RelayCommand(SelectGothic2RootDirectoryExecute);
            SelectModificationRootDirectory = new RelayCommand(SelectModificationRootDirectoryExecute);
            SaveSettings = new RelayCommand(SaveSettingsExecute);
            RestoreDefaultConfiguration = new RelayCommand(RestoreDefaultConfigurationExecute);
            OpenLogsDirectory = new RelayCommand(OpenLogsDirectoryExecute);
            OpenModBuildDirectory = new RelayCommand(OpenModBuildDirectoryExecute);
            ClearLogsDirectory = new RelayCommand(ClearLogsDirectoryExecute);
            RestoreDefaultIniOverrides = new RelayCommand(RestoreDefaultIniOverridesExecute);
            OpenGameDirectory = new RelayCommand(OpenGameDirectoryExecute);
            OpenModDirectory = new RelayCommand(OpenModDirectoryExecute);

            if (!File.Exists(GmcSettingsJsonFilePath))
                CreateDefaultConfigurationFile();

            LoadConfiguration();

            GmcConfiguration.PropertyChanged += (_, _) => SaveSettings.Execute(null);
            GmcConfiguration.IniOverrides.CollectionChanged += IniOverrides_CollectionChanged;
            GmcConfiguration.IniOverridesSystemPack.CollectionChanged += IniOverrides_CollectionChanged;

            GmcConfiguration.OnGothic2RootPathChanged += OnGothic2RootPathChanged;

            if (!string.IsNullOrWhiteSpace(GmcConfiguration.Gothic2RootPath))
                OnGothic2RootPathChanged(GmcConfiguration.Gothic2RootPath);

            LoadZen3DWorlds();
            LoadSubmods();
        }

        public void SubscribeOnWorldDirectoryChanges() =>
            _zenWorldsFileWatcherService.StartWatching();

        public void UnsubscribeOnWorldDirectoryChanges()
            => _zenWorldsFileWatcherService.StopWatching();

        public void LoadZen3DWorlds()
        {
            var worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += LoadZen3DWorlds_Worker;
            worker.ProgressChanged += LoadZen3DWorlds_ProgressChanged;
            worker.RunWorkerAsync();
        }
        public void LoadSubmods()
        {
            SubmodsHelper submodsHelper = new SubmodsHelper();
            submodsHelper.Main();
            Submods = submodsHelper.submods;
        }

        private void IniOverrides_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
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

            IsSystemPackAvailable =
                File.Exists(Path.Combine(GmcConfiguration.Gothic2RootPath, "System", "SystemPack.ini"));

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

            Application.Current.Dispatcher.Invoke(() =>
            {
                File.WriteAllText(GmcSettingsJsonFilePath, configurationJson);
            });
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
            => _gmcDirectoryService.OpenLogsDirectoryExecute(LogsDirectoryPath);

        private void ClearLogsDirectoryExecute(object obj)
        {
            _gmcDirectoryService.ClearLogsDirectoryExecute(LogsDirectoryPath);
            Application.Current.Dispatcher.Invoke(() => { IsLogDirectoryAvailable = false; });
        }

        private void OpenModBuildDirectoryExecute(object obj)
            => _gmcDirectoryService.OpenModBuildDirectoryExecute(ModBuildDirectoryPath);

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

            IsSystemPackAvailable =
                File.Exists(Path.Combine(GmcConfiguration.Gothic2RootPath, "System", "SystemPack.ini"));
        }

        private void AddMissingDefaultIniOverrides()
        {
            var defaultIniOverrideAdded = false;

            IniOverrideHelper.DefaultIniOverrideKeys.ForEach(defaultIniOverrideItem =>
            {
                if (GmcConfiguration.IniOverrides.Any(x => x.Key == defaultIniOverrideItem.Key))
                {
                    // Just to be sure if section is not filled
                    GmcConfiguration.IniOverrides.Single(x => x.Key == defaultIniOverrideItem.Key).Section =
                        defaultIniOverrideItem.Section;
                    GmcConfiguration.IniOverrides.Single(x => x.Key == defaultIniOverrideItem.Key).DisplayAs =
                        defaultIniOverrideItem.DisplayAs;
                    GmcConfiguration.IniOverrides.Single(x => x.Key == defaultIniOverrideItem.Key).AvailableValues =
                        defaultIniOverrideItem.AvailableValues;
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

        private void OnGothic2RootPathChanged(string gothic2RootPath)
        {
            var worldsPath = Path.Combine(GmcConfiguration.Gothic2RootPath, "_Work", "Data", "Worlds");

            _zenWorldsFileWatcherService.StopWatching();

            if (Directory.Exists(worldsPath))
            {
                _zenWorldsFileWatcherService.SetWorldsPath(worldsPath);
                _zenWorldsFileWatcherService.StartWatching();
            }
        }

        private void ZenWorldFilesChanged(object sender, FileSystemEventArgs e)
            => LoadZen3DWorlds();

        private void LoadZen3DWorlds_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Zen3DWorldsLoadingProgress = e.ProgressPercentage;
        }

        private void LoadZen3DWorlds_Worker(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            worker?.ReportProgress(0);

            Application.Current.Dispatcher.Invoke(() => { Zen3DWorlds.Clear(); });

            if (GmcConfiguration.Gothic2RootPath is null)
                return;

            worker?.ReportProgress(10);

            var worldsPath = Path.Combine(GmcConfiguration.Gothic2RootPath, "_Work", "Data", "Worlds");

            if (!Directory.Exists(worldsPath))
            {
                worker?.ReportProgress(0);
                return;
            }

            worker?.ReportProgress(20);

            var worldFiles = Directory.EnumerateFiles(worldsPath, "*.ZEN", SearchOption.AllDirectories);

            worker?.ReportProgress(30);

            Parallel.ForEach(worldFiles, zenFilePath =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (!_fileService.HasBinaryContent(zenFilePath))
                        return;

                    var zenFileInfo = new FileInfo(zenFilePath);
                    Zen3DWorlds.Add(new Zen3DWorld(zenFilePath, zenFileInfo.Name));
                });
            });

            worker?.ReportProgress(50);

            Parallel.ForEach(Zen3DWorlds, zen3DWorld =>
            {
                zen3DWorld.SetAsUnselected();
                if (zen3DWorld.Path == GmcConfiguration.DefaultWorld)
                    zen3DWorld.SetAsSelected();
            });

            worker?.ReportProgress(80);

            if (Zen3DWorlds.All(x => !x.IsSelected))
                GmcConfiguration.ForceGmcDefaultWorldSetNull();

            Application.Current.Dispatcher.Invoke(() =>
            {
                Zen3DWorlds = new ObservableCollection<Zen3DWorld>(Zen3DWorlds.OrderByAlphaNumeric(x => x.Name));
            });

            worker?.ReportProgress(100);
        }

        private void OpenGameDirectoryExecute(object obj)
        {
            if (Directory.Exists(GmcConfiguration.Gothic2RootPath))
            {
                Process.Start("explorer.exe", GmcConfiguration.Gothic2RootPath);
            }
            else
            {
                MessageBox.Show("Invalid Gothic II path.");
            }
        }

        private void OpenModDirectoryExecute(object obj)
        {
            if (Directory.Exists(GmcConfiguration.ModificationRootPath))
            {
                Process.Start("explorer.exe", GmcConfiguration.ModificationRootPath);
            }
            else
            {
                MessageBox.Show("Invalid mod path.");
            }
        }
    }
}