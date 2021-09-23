namespace GothicModComposer.UI.Interfaces
{
    public interface IGmcLogsDirectoryService
    {
        void OpenLogsDirectoryExecute(string gmcLogsPath);
        void ClearLogsDirectoryExecute(string gmcLogsPath);
        bool HasFiles(string gmcLogsPath);
    }
}