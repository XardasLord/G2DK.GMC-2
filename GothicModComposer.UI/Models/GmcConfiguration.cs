using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows;
using System.Windows.Forms;
using GothicModComposer.UI.Helpers;
using Application = System.Windows.Application;

namespace GothicModComposer.UI.Models
{
    public class GmcConfiguration : ObservableVM
    {
        private string _defaultWorld;
        private string _gothic2RootPath;
        private GothicArgumentsConfiguration _gothicArguments;
        private ObservableCollection<IniOverride> _iniOverrides;
        private ObservableCollection<IniOverride> _iniOverridesSystemPack;
        private string _modificationRootPath;
        private bool _closeAfterFinish = true;
        private bool _startWithWindows = false;

        public GmcConfiguration()
        {
            GothicArguments = new GothicArgumentsConfiguration();
            IniOverrides = new ObservableCollection<IniOverride>();
            IniOverridesSystemPack = new ObservableCollection<IniOverride>();

            GothicArguments.PropertyChanged += (_, _) => OnPropertyChanged(nameof(GothicArguments));
        }

        public static IEnumerable<Resolution> AvailableResolutions => GetUserSupportedResolutions();

        public string Gothic2RootPath
        {
            get => _gothic2RootPath;
            set
            {
                SetProperty(ref _gothic2RootPath, value);
                OnGothic2RootPathChanged(value);
            }
        }

        public string ModificationRootPath
        {
            get => _modificationRootPath;
            set => SetProperty(ref _modificationRootPath, value);
        }

        public bool CloseAfterFinish
        {
            get => _closeAfterFinish;
            set => SetProperty(ref _closeAfterFinish, value);
        }

        public bool StartWithWindows
        {
            get => _startWithWindows;
            set => SetProperty(ref _startWithWindows, value);
        }
        
        public string DefaultWorld
        {
            get => _defaultWorld;
            // We ensure that binding will not set null value here directly.
            set => SetProperty(ref _defaultWorld, value ?? _defaultWorld);
        }

        public GothicArgumentsConfiguration GothicArguments
        {
            get => _gothicArguments;
            set => SetProperty(ref _gothicArguments, value);
        }

        public ObservableCollection<IniOverride> IniOverrides
        {
            get => _iniOverrides;
            set => SetProperty(ref _iniOverrides, value);
        }

        public ObservableCollection<IniOverride> IniOverridesSystemPack
        {
            get => _iniOverridesSystemPack;
            set => SetProperty(ref _iniOverridesSystemPack, value);
        }

        public GothicVdfsConfig GothicVdfsConfig { get; set; }

        public event Action<string> OnGothic2RootPathChanged = delegate { };

        public static GmcConfiguration CreateDefault()
        {
            const string defaultConfig = @"
{
    ""Gothic2RootPath"": """",
    ""ModificationRootPath"": """",
	""DefaultWorld"": null,
    ""GothicArguments"": {
        ""IsWindowMode"": false,
        ""IsDevMode"": true,
        ""IsMusicDisabled"": false,
        ""IsSoundDisabled"": false,
        ""IsReparseScript"": false,
        ""Resolution"": {
            ""Width"": 800,
            ""Height"": 600
        }
    },
    ""IniOverrides"": 
    [
        { ""Key"": ""playLogoVideos"", ""Value"": ""0""},
        { ""Key"": ""subTitles"", ""Value"": ""1""}
    ],
    ""IniOverridesSystemPack"": 
    [
        { ""Key"": ""InteractionCollision"", ""Value"": ""1""}
    ],
    ""GothicVdfsConfig"": 
    {
        ""Filename"": ""DK.mod"",
        ""Comment"": ""Gothic II - The History of Khorinis (%%D)%%N(C) 2021 SoulFire"",
        ""Directories"": 
        [
            ""_work\\Data\\Anims\\_compiled"",
            ""_work\\Data\\Meshes\\_compiled"",
            ""_work\\Data\\Textures\\_compiled"",
            ""_work\\Data\\Scripts\\_compiled"",
            ""_work\\Data\\Scripts\\Content\\Cutscene\\OU.bin"",
            ""_work\\Data\\Sound\\Speech"",
            ""_work\\Data\\Sound\\SFX"",
            ""_work\\Data\\Worlds""
        ],
        ""Include"": 
        [
        ],
        ""Exclude"": 
        [
            ""_work\\Data\\Worlds\\DK_SUBZENS\\*"",
            ""_work\\Data\\Worlds\\VOBTREE\\*"",
            ""_work\\Data\\Scripts\\Content\\Cutscene\\OU.csl""
        ]
    }
}";
            return JsonSerializer.Deserialize<GmcConfiguration>(defaultConfig);
        }

        public void ForceGmcDefaultWorldSetNull()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                _defaultWorld = null;
                OnPropertyChanged(nameof(DefaultWorld));
            });
        }

        private static IEnumerable<Resolution> GetUserSupportedResolutions()
        {
            var supportedResolutions = new List<Resolution>
            {
                new() {Width = 800, Height = 600},
                new() {Width = 1024, Height = 768},
                new() {Width = 1280, Height = 720},
                new() {Width = 1280, Height = 1024},
                new() {Width = 1366, Height = 768},
                new() {Width = 1600, Height = 900},
                new() {Width = 1600, Height = 1200},
                new() {Width = 1920, Height = 1080},
                new() {Width = 2048, Height = 1152},
                new() {Width = 2560, Height = 1440},
                new() {Width = 3840, Height = 2160}
            };

            foreach (var supportedResolution in supportedResolutions)
            {
                if (supportedResolution.Height <= Screen.PrimaryScreen.Bounds.Height &&
                    supportedResolution.Width <= Screen.PrimaryScreen.Bounds.Width)
                {
                    yield return supportedResolution;
                }
            }
        }
    }
}