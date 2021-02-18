using System.Collections.Generic;
using GothicModComposer.Commands;

namespace GothicModComposer.Models.Profiles
{
	public class ProfileLoaderResponse
	{
		public IProfile Profile { get; set; }
		public List<ICommand> Commands { get; set; }
	}
}