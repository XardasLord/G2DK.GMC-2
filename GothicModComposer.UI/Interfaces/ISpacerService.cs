namespace GothicModComposer.UI.Interfaces
{
    public interface ISpacerService
    {
        bool SpacerExists(string gothicRootPath);
        void RunSpacer(string gothicRootPath);
    }
}