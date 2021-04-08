using System.Collections.Generic;
using GothicModComposer.Models.Interfaces;
using GothicModComposer.Utils;

namespace GothicModComposer.Models.Profiles
{
    public class Profile : IProfile
	{
		public IGothicFolder GothicFolder { get; set; }
		public IGmcFolder GmcFolder { get; set; }
		public IModFolder ModFolder { get; set; }
		public string DefaultWorld { get; set; }
		public List<string> IniOverrides { get; set; }
		public IGothicArguments GothicArguments { get; set; }
        public IGothicVdfsConfig GothicVdfsConfig { get; set; }
        public ICommandsConditions CommandsConditions { get; set; }
        public ILoggerService LoggerService { get; set; }
	}

	public interface IProfile
	{
		IGothicFolder GothicFolder { get; set; }
		IGmcFolder GmcFolder { get; set; }
		IModFolder ModFolder { get; set; }
		string DefaultWorld { get; set; }
		List<string> IniOverrides { get; set; }
		IGothicArguments GothicArguments { get; set; }
		IGothicVdfsConfig GothicVdfsConfig { get; set; }
        ICommandsConditions CommandsConditions { get; set; }
		ILoggerService LoggerService { get; set; }
	}
}