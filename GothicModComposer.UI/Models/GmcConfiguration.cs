using System.Collections.Generic;
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

        public string Gothic2RootPath
        {
            get => _gothic2RootPath;
            set => SetProperty(ref _gothic2RootPath, value);
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

        public List<string> IniOverrides { get; set; }
        public GothicVdfsConfig GothicVdfsConfig { get; set; }

        public static GmcConfiguration CreateDefault()
        {
            const string defaultConfig = @"
{
	""DefaultWorld"": ""VADUZWORLD.ZEN"",

    ""IniOverrides"": 
    [

        ""playLogoVideos = 0"",
        ""subTitles = 1""
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
    }
}