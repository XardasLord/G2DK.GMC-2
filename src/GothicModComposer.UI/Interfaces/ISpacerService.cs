using System.Diagnostics;

namespace GothicModComposer.UI.Interfaces
{
    public interface ISpacerService
    {
        bool SpacerExists(string gothicRootPath);
        Process RunSpacer(string gothicRootPath);
    }
}