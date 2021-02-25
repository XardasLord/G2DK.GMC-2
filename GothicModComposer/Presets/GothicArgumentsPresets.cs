using GothicModComposer.Models.Profiles;

namespace GothicModComposer.Presets
{
	public static class GothicArgumentsPresets
    {
	    /// <summary>
        /// Default set of empty arguments.
        /// </summary>
        public static GothicArguments Default()
        {
            return GothicArguments.Empty();
        }

        /// <summary>Returns gothic arguments configuration with attributes:
        /// <para>--nosound --znomusic</para>
        /// </summary>
        public static GothicArguments Compile()
        {
            return GothicArguments.Empty()
                .ZWindow()
                ._3D("compile")
                .ZRes()
                .ZLog()
                .NoMenu()
                .ZNoTex()
                .ZNoMusic()
                .ZNoSound();
        }

        /// <summary>Returns gothic arguments configuration with attributes:
        /// <para>--nosound --znomusic</para>
        /// </summary>
        public static GothicArguments NoSounds()
        {
            return GothicArguments.Empty()
                .ZNoSound()
                .ZNoMusic();
        }

        /// <summary>Returns gothic arguments configuration with attributes:
        /// <para>--zconvertall --zreparse --ztexconvert</para>
        /// </summary>
        public static GothicArguments RecompileAll()
        {
            return GothicArguments.Empty()
                .ZConvertAll()
                .ZReparse()
                .ZTexConvert();
        }

        /// <summary>Returns gothic arguments configuration with attributes:
        /// <para>--zwindowed --zlog:5,s --zres:800,600,32 --nomenu</para>
        /// </summary>
        public static GothicArguments Build()
        {
            return GothicArguments.Empty()
                .ZWindow()
                .ZLog()
                .ZRes()
                .NoMenu();
        }

        /// <summary>Returns gothic arguments configuration with attributes:
        /// <para>--zwindowed --devmode --zlog:5,s --zres:800,600,32</para>
        /// </summary>
        public static GothicArguments Run()
        {
            return GothicArguments.Empty()
                .ZWindow()
                .DevMode()
                .ZRes()
                .Vdfs();
        }

        /// <summary>Returns gothic arguments configuration with attributes:
        /// <para>--zreparse --zwindowed --vdfs:physicalfirst --devmode</para>
        /// </summary>
        public static GothicArguments Test()
        {
            return GothicArguments.Empty()
                .ZWindow()
                .ZRes()
                .NoMenu()
                ._3D()
                .ZLog();
        }

    }
}