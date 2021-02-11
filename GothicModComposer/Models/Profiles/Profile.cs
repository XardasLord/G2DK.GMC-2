﻿using System.Collections.Generic;
using GothicModComposer.Models.Folders;

namespace GothicModComposer.Models.Profiles
{
	public class Profile : IProfile
	{
		public GothicFolder GothicFolder { get; set; }
		public GmcFolder GmcFolder { get; set; }
		public ModFolder ModFolder { get; set; }
		public string DefaultWorld { get; set; }
		public List<string> IniOverrides { get; set; }
		public GothicArguments GothicArguments { get; set; }
	}

	public interface IProfile
	{
		GothicFolder GothicFolder { get; set; }
		GmcFolder GmcFolder { get; set; }
		ModFolder ModFolder { get; set; }
		string DefaultWorld { get; set; }
		List<string> IniOverrides { get; set; }
		GothicArguments GothicArguments { get; set; }
	}
}