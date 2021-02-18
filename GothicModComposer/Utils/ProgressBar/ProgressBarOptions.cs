using System;
using ShellProgressBar;

namespace GothicModComposer.Utils.ProgressBar
{
	public static class ProgressBarOptionsHelper
	{
		public static ProgressBarOptions Get()
		{
			return new()
			{
				ForegroundColor = ConsoleColor.Yellow,
				ForegroundColorDone = ConsoleColor.Cyan,
				BackgroundColor = ConsoleColor.DarkGray,
				BackgroundCharacter = '\u2593',
			};
		}
	}
}