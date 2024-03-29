﻿using GothicModComposer.Models.Profiles;

namespace GothicModComposer.Presets
{
    public static class GothicArgumentsPresets
    {
        /// <summary>
        ///     Default set of empty arguments.
        /// </summary>
        public static GothicArguments Default() => GothicArguments.Empty();

        /// <summary>
        ///     Returns gothic arguments configuration with attributes:
        ///     <para>--nosound --znomusic</para>
        /// </summary>
        public static GothicArguments Compile() =>
            GothicArguments.Empty()
                .AddArgument_ZWindow()
                .AddArgument_3D("compile")
                .AddArgument_ZRes()
                .AddArgument_ZLog()
                .AddArgument_NoMenu()
                .AddArgument_ZNoTex()
                .AddArgument_ZNoMusic()
                .AddArgument_ZNoSound();

        /// <summary>
        ///     Returns gothic arguments configuration with attributes:
        ///     <para>--nosound --znomusic</para>
        /// </summary>
        public static GothicArguments NoSounds() =>
            GothicArguments.Empty()
                .AddArgument_ZNoSound()
                .AddArgument_ZNoMusic();

        /// <summary>
        ///     Returns gothic arguments configuration with attributes:
        ///     <para>--zconvertall --zreparse --ztexconvert</para>
        /// </summary>
        public static GothicArguments RecompileAll() =>
            GothicArguments.Empty()
                .AddArgument_ZConvertAll()
                .AddArgument_ZReparse()
                .AddArgument_ZTexConvert();

        /// <summary>
        ///     Returns gothic arguments configuration with attributes:
        ///     <para>--zwindowed --zlog:5,s --zres:800,600,32 --nomenu</para>
        /// </summary>
        public static GothicArguments Build() =>
            GothicArguments.Empty()
                .AddArgument_ZWindow()
                .AddArgument_ZLog()
                .AddArgument_ZRes()
                .AddArgument_NoMenu();

        /// <summary>
        ///     Returns gothic arguments configuration with attributes:
        ///     <para>--zwindowed --zlog:5,s --zres:800,600,32 --nomenu</para>
        /// </summary>
        public static GothicArguments Compose() =>
            GothicArguments.Empty()
                .AddArgument_ZWindow()
                .AddArgument_ZLog()
                .AddArgument_ZRes()
                .AddArgument_NoMenu()
                .AddArgument_ZReparse()
                .AddArgument_ZConvertAll()
                .AddArgument_ZTexConvert();

        /// <summary>
        ///     Returns gothic arguments configuration with attributes:
        ///     <para>--zwindowed --devmode --zlog:5,s --zres:800,600,32</para>
        /// </summary>
        public static GothicArguments Run() =>
            GothicArguments.Empty()
                .AddArgument_ZWindow()
                .AddArgument_DevMode()
                .AddArgument_ZRes()
                .AddArgument_Vdfs();

        /// <summary>
        ///     Returns gothic arguments configuration with attributes:
        ///     <para>--zreparse --zwindowed --vdfs:physicalfirst --devmode</para>
        /// </summary>
        public static GothicArguments Test() =>
            GothicArguments.Empty()
                .AddArgument_ZWindow()
                .AddArgument_ZRes()
                .AddArgument_NoMenu()
                .AddArgument_3D()
                .AddArgument_ZLog();
    }
}