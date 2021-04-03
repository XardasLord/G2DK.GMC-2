using System.Collections.Generic;
using System.Linq;

namespace GothicModComposer.Models.IniFiles
{
	public class IniBlock
	{
        public string Header { get; }
		public List<KeyValuePair<string, string>> Properties => _settings.ToList();

        private readonly Dictionary<string, string> _settings = new();

		public IniBlock(string header)
		{
			Header = header;
		}

		public bool Contains(string key) 
			=> _settings.Keys.Contains(key);

		public void Set(string key, string value) 
			=> _settings[key] = value;
	}
}