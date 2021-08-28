using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using GothicModComposer.UI.Helpers;

namespace GothicModComposer.UI.Models
{
    public class GmcConfiguration : ObservableVM
    {
        private string _gothic2RootPath;
        private string _modificationRootPath;
        private string _defaultWorld;
        private GothicArgumentsConfiguration _gothicArguments;
        private ObservableCollection<IniOverride> _iniOverrides;
        private ObservableCollection<IniOverride> _iniOverridesSystemPack;

        public event Action<string> OnGothic2RootPathChanged = delegate(string s) { };

        public static IEnumerable<Resolution> AvailableResolutions => new List<Resolution>
        {
            new Resolution { Width = 640, Height = 480 },
            new Resolution { Width = 800, Height = 600 },
            new Resolution { Width = 1024, Height = 768 },
            new Resolution { Width = 1280, Height = 720 },
            new Resolution { Width = 1280, Height = 1024 },
            new Resolution { Width = 1366, Height = 768 },
            new Resolution { Width = 1600, Height = 900 },
            new Resolution { Width = 1600, Height = 1200 },
            new Resolution { Width = 1920, Height = 1080 },
            new Resolution { Width = 2048, Height = 1152 },
            new Resolution { Width = 2048, Height = 1536 },
            new Resolution { Width = 2560, Height = 1440 },
        };

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

        public string DefaultWorld
        {
            get => _defaultWorld;
            set => SetProperty(ref _defaultWorld, value);
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

        public GmcConfiguration()
        {
            GothicArguments = new GothicArgumentsConfiguration();
            IniOverrides = new ObservableCollection<IniOverride>();
            IniOverridesSystemPack = new ObservableCollection<IniOverride>();

            GothicArguments.PropertyChanged += (_, _) => OnPropertyChanged(nameof(GothicArguments));
        }
    }

    public class GothicVdfsConfig
    {
        public string Filename { get; set; }
        public List<string> Directories { get; set; }
        public List<string> Include { get; set; }
        public List<string> Exclude { get; set; }
        public string Comment { get; set; }
    }

    public class GothicArgumentsConfiguration : ObservableVM
    {
        private bool _isWindowMode;
        private bool _isDevMode;
        private bool _isMusicDisabled;
        private bool _isSoundDisabled;
        private bool _isReparseScripts;
        private Resolution _resolution;

        public bool IsWindowMode
        {
            get => _isWindowMode;
            set => SetProperty(ref _isWindowMode, value);
        }

        public bool IsDevMode
        {
            get => _isDevMode;
            set => SetProperty(ref _isDevMode, value);
        }

        public bool IsMusicDisabled
        {
            get => _isMusicDisabled;
            set => SetProperty(ref _isMusicDisabled, value);
        }

        public bool IsSoundDisabled
        {
            get => _isSoundDisabled;
            set => SetProperty(ref _isSoundDisabled, value);
        }

        public bool IsReparseScript
        {
            get => _isReparseScripts;
            set => SetProperty(ref _isReparseScripts, value);
        }

        public Resolution Resolution
        {
            get => _resolution;
            set => SetProperty(ref _resolution, value);
        }
    }

    public class Resolution
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public override string ToString()
        {
            return $"{Width}x{Height}";
        }
        
        public override bool Equals(object obj)
        {
            if (obj is not Resolution resolution)
                return false;
            
            return resolution.Height == Height && resolution.Width == Width;
        }

        public override int GetHashCode()
        {
            return Height.GetHashCode() + Width.GetHashCode();
        }
    }
}