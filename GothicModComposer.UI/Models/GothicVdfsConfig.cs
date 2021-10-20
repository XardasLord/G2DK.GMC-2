using System.Collections.Generic;

namespace GothicModComposer.UI.Models
{
    public class GothicVdfsConfig
    {
        public string Filename { get; set; }
        public List<string> Directories { get; set; }
        public List<string> Include { get; set; }
        public List<string> Exclude { get; set; }
        public string Comment { get; set; }
    }
}