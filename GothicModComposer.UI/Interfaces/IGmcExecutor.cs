using GothicModComposer.UI.Enums;
using GothicModComposer.UI.ViewModels;

namespace GothicModComposer.UI.Interfaces
{
    public interface IGmcExecutor
    {
        void Execute(GmcExecutionProfile profile, GmcSettingsVM settingsVM);
    }
}