using System.Collections.Generic;
using GothicModComposer.Models.Interfaces;

namespace GothicModComposer.Models.Vdfs
{
    public class GothicVdfsConfig : IGothicVdfsConfig
    {
        public string Filename { get; set; }
        public List<string> Directories { get; set; }
        public List<string> Include { get; set; }
        public List<string> Exclude { get; set; }
        public string Comment { get; set; }
    }
}