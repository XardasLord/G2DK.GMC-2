using System.Collections.Generic;

namespace GothicModComposer.UI.Models
{
    public class GmcConfiguration
    {
        public string DefaultWorld { get; }
        public List<string> IniOverrides { get; }
        public GothicVdfsConfig GothicVdfsConfig { get; }

        public static GmcConfiguration CreateDefault()
        {
            //TODO: Create default json configuration file under this path
            return new GmcConfiguration();
        }

        private GmcConfiguration()
        {
        }
    }

    public class GothicVdfsConfig
    {
        public string Filename { get; set; }
        public List<string> Directories { get; set; }
        public List<string> Include { get; set; }
        public List<string> Exclude { get; set; }
        public string Comment { get; set; }
    }
}