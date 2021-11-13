using GothicModComposer.Core.Models.Interfaces;

namespace GothicModComposer.Core.Models.Configurations
{
    public class GothicArgumentsConfiguration : IGothicArgumentsConfiguration
    {
        public bool IsWindowMode { get; set; }
        public bool IsDevMode { get; set; }
        public bool IsMusicDisabled { get; set; }
        public bool IsSoundDisabled { get; set; }
        public bool IsReparseScript { get; set; }
        public bool IsConvertTextures { get; set; }
        public bool IsConvertData { get; set; }
        public bool IsConvertAll { get; set; }
        public Resolution Resolution { get; set; }
    }
}