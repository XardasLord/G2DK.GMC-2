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
        Resolution Resolution { get; set; }
    }
}