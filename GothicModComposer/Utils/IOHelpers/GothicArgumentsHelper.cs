using System.Collections.Generic;
using System.Linq;
using GothicModComposer.Models.Profiles;

namespace GothicModComposer.Utils.IOHelpers
{
	public static class GothicArgumentsHelper
	{
		public static GothicArguments ParseGothicArguments(List<string> arguments)
		{
			var gothicArguments = GothicArguments.Empty();

			if (arguments is null)
				return gothicArguments;

			arguments.ForEach(argument =>
			{
				var arg = argument.Split(':');
				gothicArguments.SetArg(arg.ElementAtOrDefault(0), arg.ElementAtOrDefault(1));
			});

			return gothicArguments;
		}
	}
}