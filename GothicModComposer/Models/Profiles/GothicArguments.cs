using System.Collections.Generic;
using System.Linq;
using System.Text;
using GothicModComposer.Models.Interfaces;

namespace GothicModComposer.Models.Profiles
{
    public class GothicArguments : IGothicArguments
    {
        // TODO: We need to provide RemoveArgument_XXX for all arguments instead of calling them directly by key
        public const string ZConvertAllParameter = "ZCONVERTALL";
        public const string ZReparseParameter = "ZREPARSE";
        public const string ReparseVisParameter = "REPARSEVIS";
        public const string ZTexConvertParameter = "ZTEXCONVERT";
        public const string ZWindowModeParameter = "ZWINDOW";
        public const string DevModeParameter = "DEVMODE";

        private readonly Dictionary<string, string> _gothicArguments;

        private GothicArguments() => _gothicArguments = new Dictionary<string, string>();

        private GothicArguments(IEnumerable<KeyValuePair<string, string>> args) => _gothicArguments =
            new Dictionary<string, string>((IDictionary<string, string>) args);

        public bool Contains(string parameter)
            => _gothicArguments.ContainsKey(parameter);

        public void SetArg(string argument, string value = null)
        {
            _gothicArguments[argument.ToUpper()] = value;
        }

        public bool RemoveArg(string argument)
            => _gothicArguments.Remove(argument.ToUpper());

        public List<string> ToList()
        {
            return _gothicArguments.Select(item => item.Value == null ? $"{item.Key}" : $"{item.Key}:{item.Value}")
                .ToList();
        }

        public GothicArguments Merge(IGothicArgumentsConfiguration profileGothicArgumentsForceConfig)
        {
            if (profileGothicArgumentsForceConfig is null)
                // If we don't pass config (e.g. running from bat files)
                return this;

            if (profileGothicArgumentsForceConfig.IsWindowMode)
                SetArg(ZWindowModeParameter);
            else
                RemoveArg(ZWindowModeParameter);

            if (profileGothicArgumentsForceConfig.IsDevMode)
                SetArg(DevModeParameter);
            else
                RemoveArg(DevModeParameter);

            if (profileGothicArgumentsForceConfig.IsMusicDisabled)
                AddArgument_ZNoMusic();
            else
                RemoveArg("ZNOMUSIC");

            if (profileGothicArgumentsForceConfig.IsSoundDisabled)
                AddArgument_ZNoSound();
            else
                RemoveArg("ZNOSOUND");

            if (profileGothicArgumentsForceConfig.IsReparseScript)
                AddArgument_ZReparse();
            else
                RemoveArg("ZREPARSE");

            if (profileGothicArgumentsForceConfig.Resolution is null)
                AddArgument_ZRes();
            else
                AddArgument_ZRes(
                    profileGothicArgumentsForceConfig.Resolution.Width.ToString(),
                    profileGothicArgumentsForceConfig.Resolution.Height.ToString());

            return this;
        }

        public override string ToString()
        {
            var args = new StringBuilder();

            foreach (var key in _gothicArguments.Keys)
                args.Append(_gothicArguments[key] == null ? $"-{key} " : $"-{key}:{_gothicArguments[key]} ");

            return args.ToString();
        }

        /// <summary>MISSING SUMMARY</summary>
        public GothicArguments AddArgument_ZConvertAll()
        {
            SetArg("ZCONVERTALL");
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments AddArgument_Player(string value = null)
        {
            SetArg("PLAYER", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments AddArgument_ZNoFastExit(string value = null)
        {
            SetArg("ZNOFASTEXIT", value);
            return this;
        }

        /// <summary>Game skips all the menus and starts the New Game on startup. In game `ESC` button closes the game.</summary>
        public GothicArguments AddArgument_NoMenu()
        {
            SetArg("NOMENU");
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments AddArgument_Parse(string value = null)
        {
            SetArg("PARSE", value);
            return this;
        }

        /// <summary>What world should be loaded after selecting new game. Value eg. "NewWorld.zen"</summary>
        public GothicArguments AddArgument_3D(string value = "none")
        {
            SetArg("3D", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments AddArgument_Ini(string value = null)
        {
            SetArg("ini", value);
            return this;
        }

        /// <summary>Starts the game in "MARVIN" mode.</summary>
        public GothicArguments AddArgument_DevMode()
        {
            SetArg("DEVMODE");
            return this;
        }

        /// <summary>Sets the active modification. Value eg. "MyMod.ini"</summary>
        public GothicArguments AddArgument_Game(string value = "gothic.ini")
        {
            SetArg("GAME", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments AddArgument_Vdfs(string value = "physicalfirst")
        {
            SetArg("VDFS", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments AddArgument_PhysicalFirst(string value = null)
        {
            SetArg("PHYSICALFIRST", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments AddArgument_CheckRoutines(string value = null)
        {
            SetArg("CHECKROUTINES", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments AddArgument_ReparseVis(string value = null)
        {
            SetArg(ReparseVisParameter, value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments AddArgument_ConvertDialogCams(string value = null)
        {
            SetArg("CONVERTDIALOGCAMS", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments AddArgument_ZSector(string value = null)
        {
            SetArg("ZSECTOR", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments AddArgument_ZSkipSector(string value = null)
        {
            SetArg("ZSKIPSECTORS", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments AddArgument_ZMemProfiler(string value = null)
        {
            SetArg("ZMEMPROFILER", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments AddArgument_ZNoResThread(string value = null)
        {
            SetArg("ZNORESTHREAD", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments AddArgument_ZTexConvert(string value = null)
        {
            SetArg("ZTEXCONVERT", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments AddArgument_ZLoad3DSoldMethod(string value = null)
        {
            SetArg("ZLOAD3DSOLDMETHOD", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments AddArgument_ZenSaveOldMethod(string value = null)
        {
            SetArg("ZENSAVEOLDMETHOD", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments AddArgument_ZNoSharedFeatures(string value = null)
        {
            SetArg("ZNOSHAREDFEATURES", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments AddArgument_ZNoPFX(string value = null)
        {
            SetArg("ZNOPFX", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments AddArgument_ZTexMaxSize(string value = null)
        {
            SetArg("ZTEXMAXSIZE", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments AddArgument_MergeVobsWithLevel(string value = null)
        {
            SetArg("MERGEVOBSWITHLEVEL", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments AddArgument_ZMaxFrameRate(string value = null)
        {
            SetArg("ZMAXFRAMERATE", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments AddArgument_ZNoMemPool(string value = null)
        {
            SetArg("ZNOMEMPOOL", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments AddArgument_ZRnd(string value = null)
        {
            SetArg("ZRND", value);
            return this;
        }

        /// <summary>Starts the game with predefined resolution.</summary>
        public GothicArguments AddArgument_ZRes(string width = "800", string height = "600", string bpp = "32")
        {
            SetArg("ZRES", $"{width},{height},{bpp}");
            return this;
        }

        /// <summary>Starts the game in windowed mode.</summary>
        public GothicArguments AddArgument_ZWindow()
        {
            SetArg("ZWINDOW");
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments AddArgument_ZNoTex(string value = null)
        {
            SetArg("ZNOTEX", value);
            return this;
        }

        /// <summary>Starts the game with no music enabled.</summary>
        public GothicArguments AddArgument_ZNoMusic()
        {
            SetArg("ZNOMUSIC");
            return this;
        }

        /// <summary>Starts the game without sound effects.</summary>
        public GothicArguments AddArgument_ZNoSound()
        {
            SetArg("ZNOSOUND");
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments AddArgument_ZAutoConvertData(string value = null)
        {
            SetArg("ZAUTOCONVERTDATA", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments AddArgument_ZAccount(string value = null)
        {
            SetArg("ZACCOUNT", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments AddArgument_ZAccountFull(string value = null)
        {
            SetArg("ZACCOUNTFULL", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments AddArgument_DebugFightAI(string value = null)
        {
            SetArg("DEBUGFIGHTAI", value);
            return this;
        }

        /// <summary>Sets the startup in game time to this value.</summary>
        public GothicArguments AddArgument_Time(string hour = "00", string minute = "00")
        {
            SetArg("TIME", $"{hour}:{minute}");
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments AddArgument_ClipRange(string value = null)
        {
            SetArg("CLIPRANGE", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments AddArgument_FogRange(string value = null)
        {
            SetArg("FOGRANGE", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments AddArgument_CamLightRange(string value = null)
        {
            SetArg("CAMLIGHTRANGE", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments AddArgument_LightRange(string value = null)
        {
            SetArg("LIGHTRANGE", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments AddArgument_OutDoor(string value = null)
        {
            SetArg("OUTDOOR", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments AddArgument_NoLazyLoad(string value = null)
        {
            SetArg("NOLAZYLOAD", value);
            return this;
        }

        /// <summary>MISSING SUMMARY</summary>
        public GothicArguments AddArgument_ZReparse()
        {
            SetArg(ZReparseParameter);
            return this;
        }

        /// <summary>MISSING SUMMARY</summary>
        public GothicArguments AddArgument_ZLog(int value = 5)
        {
            SetArg("ZLOG", $"{value},s");
            return this;
        }

        public static GothicArguments operator +(GothicArguments o1, GothicArguments o2)
        {
            var result = new GothicArguments(o1._gothicArguments);
            foreach (var key in o2._gothicArguments.Keys) result._gothicArguments[key] = o2._gothicArguments[key];

            return result;
        }

        public static GothicArguments operator -(GothicArguments o1, GothicArguments o2)
        {
            var result = new GothicArguments(o1._gothicArguments);
            foreach (var key in o2._gothicArguments.Keys)
                if (result._gothicArguments.ContainsKey(key))
                    result._gothicArguments.Remove(key);

            return result;
        }

        /// <summary>Returns new empty GothicArguments Object</summary>
        public static GothicArguments Empty() => new GothicArguments();
    }
}