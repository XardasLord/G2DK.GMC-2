using System;
using ShellProgressBar;

namespace GothicModComposer.Core.Utils.ProgressBar
{
    public static class ProgressBarOptionsHelper
    {
        public static ProgressBarOptions Get() =>
            new ProgressBarOptions
            {
                ForegroundColor = ConsoleColor.Yellow,
                ForegroundColorDone = ConsoleColor.Cyan,
                BackgroundColor = ConsoleColor.DarkGray,
                BackgroundCharacter = '\u2593'
            };
    }
}