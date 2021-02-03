using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GothicModComposer.Models.Profiles
{
    public class GothicArguments
    {
	    private readonly Dictionary<string, string> _gothicArguments;

        private GothicArguments()
        {
            _gothicArguments = new Dictionary<string, string>();
        }

        private GothicArguments(IEnumerable<KeyValuePair<string, string>> args)
        {
            _gothicArguments = new Dictionary<string, string>((IDictionary<string, string>)args);
        }

        public void SetArg(string argument, string value = null)
        {
            _gothicArguments[argument.ToUpper()] = value;
        }

        public bool RemoveArg(string argument)
        {
            return _gothicArguments.Remove(argument.ToUpper());
        }

        public static GothicArguments operator +(GothicArguments o1, GothicArguments o2)
        {
            GothicArguments result = new GothicArguments(o1._gothicArguments);
            foreach (var key in o2._gothicArguments.Keys)
            {
                result._gothicArguments[key] = o2._gothicArguments[key];
            }

            return result;
        }

        public static GothicArguments operator -(GothicArguments o1, GothicArguments o2)
        {
            GothicArguments result = new GothicArguments(o1._gothicArguments);
            foreach (var key in o2._gothicArguments.Keys)
            {
                if (result._gothicArguments.ContainsKey(key))
                {
                    result._gothicArguments.Remove(key);
                }
            }

            return result;
        }

        public List<string> ToList()
        {
            return _gothicArguments.Select(item => item.Value == null ? $"{item.Key}" : $"{item.Key}:{item.Value}")
                .ToList();
        }

        public override string ToString()
        {
            StringBuilder args = new StringBuilder();

            foreach (var key in _gothicArguments.Keys)
            {
                args.Append(_gothicArguments[key] == null ? $"-{key} " : $"-{key}:{_gothicArguments[key]} ");
            }

            return args.ToString();
        }

        /// <summary>Returns new empty GothicArguments Object</summary>
        public static GothicArguments Empty()
        {
            return new GothicArguments();
        }

        /// <summary>MISSING SUMMARY</summary>
        public GothicArguments ZConvertAll()
        {
            SetArg("ZCONVERTALL");
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments Player(string value = null)
        {
            SetArg("PLAYER", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments ZNoFastExit(string value = null)
        {
            SetArg("ZNOFASTEXIT", value);
            return this;
        }

        /// <summary>Game skips all the menus and starts the New Game on startup. In game `ESC` button closes the game.</summary>
        public GothicArguments NoMenu()
        {
            SetArg("NOMENU");
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments Parse(string value = null)
        {
            SetArg("PARSE", value);
            return this;
        }

        /// <summary>What world should be loaded after selecting new game. Value eg. "NewWorld.zen"</summary>
        public GothicArguments _3D(string value = "none")
        {
            SetArg("3D", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments Ini(string value = null)
        {
            SetArg("ini", value);
            return this;
        }

        /// <summary>Starts the game in "MARVIN" mode.</summary>
        public GothicArguments DevMode()
        {
            SetArg("DEVMODE");
            return this;
        }

        /// <summary>Sets the active modification. Value eg. "MyMod.ini"</summary>
        public GothicArguments Game(string value = "gothic.ini")
        {
            SetArg("GAME", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments Vdfs(string value = "physicalfirst")
        {
            SetArg("VDFS", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments PhysicalFirst(string value = null)
        {
            SetArg("PHYSICALFIRST", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments CheckRoutines(string value = null)
        {
            SetArg("CHECKROUTINES", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments ReparseVis(string value = null)
        {
            SetArg("REPARSEVIS", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments ConvertDialogCams(string value = null)
        {
            SetArg("CONVERTDIALOGCAMS", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments ZSector(string value = null)
        {
            SetArg("ZSECTOR", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments ZSkipSector(string value = null)
        {
            SetArg("ZSKIPSECTORS", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments ZMemProfiler(string value = null)
        {
            SetArg("ZMEMPROFILER", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments ZNoResThread(string value = null)
        {
            SetArg("ZNORESTHREAD", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments ZTexConvert(string value = null)
        {
            SetArg("ZTEXCONVERT", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments ZLoad3DSoldMethod(string value = null)
        {
            SetArg("ZLOAD3DSOLDMETHOD", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments ZenSaveOldMethod(string value = null)
        {
            SetArg("ZENSAVEOLDMETHOD", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments ZNoSharedFeatures(string value = null)
        {
            SetArg("ZNOSHAREDFEATURES", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments ZNoPFX(string value = null)
        {
            SetArg("ZNOPFX", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments ZTexMaxSize(string value = null)
        {
            SetArg("ZTEXMAXSIZE", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments MergeVobsWithLevel(string value = null)
        {
            SetArg("MERGEVOBSWITHLEVEL", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments ZMaxFrameRate(string value = null)
        {
            SetArg("ZMAXFRAMERATE", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments ZNoMemPool(string value = null)
        {
            SetArg("ZNOMEMPOOL", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments ZRnd(string value = null)
        {
            SetArg("ZRND", value);
            return this;
        }

        /// <summary>Starts the game with predefined resolution.</summary>
        public GothicArguments ZRes(string width = "800", string height = "600", string bpp = "32")
        {
            SetArg("ZRES", $"{width},{height},{bpp}");
            return this;
        }

        /// <summary>Starts the game in windowed mode.</summary>
        public GothicArguments ZWindow()
        {
            SetArg("ZWINDOW");
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments ZNoTex(string value = null)
        {
            SetArg("ZNOTEX", value);
            return this;
        }

        /// <summary>Starts the game with no music enabled.</summary>
        public GothicArguments ZNoMusic()
        {
            SetArg("ZNOMUSIC");
            return this;
        }

        /// <summary>Starts the game without sound effects.</summary>
        public GothicArguments ZNoSound()
        {
            SetArg("ZNOSOUND");
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments ZAutoConvertData(string value = null)
        {
            SetArg("ZAUTOCONVERTDATA", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments ZAccount(string value = null)
        {
            SetArg("ZACCOUNT", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments ZAccountFull(string value = null)
        {
            SetArg("ZACCOUNTFULL", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments DebugFightAI(string value = null)
        {
            SetArg("DEBUGFIGHTAI", value);
            return this;
        }

        /// <summary>Sets the startup in game time to this value.</summary>
        public GothicArguments Time(string hour = "00", string minute = "00")
        {
            SetArg("TIME", $"{hour}:{minute}");
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments ClipRange(string value = null)
        {
            SetArg("CLIPRANGE", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments FogRange(string value = null)
        {
            SetArg("FOGRANGE", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments CamLightRange(string value = null)
        {
            SetArg("CAMLIGHTRANGE", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments LightRange(string value = null)
        {
            SetArg("LIGHTRANGE", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments OutDoor(string value = null)
        {
            SetArg("OUTDOOR", value);
            return this;
        }

        /// <summary>MISSING SUMMARY: NOT KNOWN ATTRIBUTE</summary>
        public GothicArguments NoLazyLoad(string value = null)
        {
            SetArg("NOLAZYLOAD", value);
            return this;
        }

        /// <summary>MISSING SUMMARY</summary>
        public GothicArguments ZReparse()
        {
            SetArg("ZREPARSE");
            return this;
        }

        /// <summary>MISSING SUMMARY</summary>
        public GothicArguments ZLog(int value = 5)
        {
            SetArg("ZLOG", $"{value},s");
            return this;
        }

    }
}