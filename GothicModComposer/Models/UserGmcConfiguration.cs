using System.Collections.Generic;

namespace GothicModComposer.Models
{
	public class UserGmcConfiguration
	{
		public string GothicRoot { get; set; }
		public string DefaultWorld { get; set; }
		public List<string> IniOverrides { get; set; }
	}
}