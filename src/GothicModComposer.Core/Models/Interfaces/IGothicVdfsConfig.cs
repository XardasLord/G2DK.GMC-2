using System.Collections.Generic;

namespace GothicModComposer.Core.Models.Interfaces
{
    public interface IGothicVdfsConfig
    {
        string Filename { get; set; }
        List<string> Directories { get; set; }
        List<string> Include { get; set; }
        List<string> Exclude { get; set; }
        string Comment { get; set; }
    }
}