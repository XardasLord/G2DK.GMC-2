using GothicModComposer.UI.Enums;
using GothicModComposer.UI.ViewModels;

namespace GothicModComposer.UI.Interfaces
{
    public interface IGmcExecutor
    {
        bool GothicExecutableExists(string gothicRootPath);
        bool GothicVdfsExecutableExists(string gothicRootPath);
        void Execute(GmcExecutionProfile profile, GmcSettingsVM gmcSettingsVM);
    }
}