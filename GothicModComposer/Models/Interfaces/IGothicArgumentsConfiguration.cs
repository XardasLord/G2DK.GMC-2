using GothicModComposer.Models.Configurations;

namespace GothicModComposer.Models.Interfaces
{
    public interface IGothicArgumentsConfiguration
    {
        bool IsWindowMode { get; set; }
        bool IsDevMode { get; set; }
        bool IsMusicDisabled { get; set; }
        bool IsSoundDisabled { get; set; }
        bool IsReparseScript { get; set; }
        bool IsConvertTextures { get; set; }
        bool IsConvertData { get; set; }
        bool IsConvertAll { get; set; }
        Resolution Resolution { get; set; }
    }
}