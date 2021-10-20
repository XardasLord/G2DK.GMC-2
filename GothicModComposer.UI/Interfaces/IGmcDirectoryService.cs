namespace GothicModComposer.UI.Interfaces
{
    public interface IGmcDirectoryService
    {
        void OpenLogsDirectoryExecute(string gmcLogsPath);
        void ClearLogsDirectoryExecute(string gmcLogsPath);
        bool HasFiles(string gmcLogsPath);
        void OpenModBuildDirectoryExecute(string modBuildDirectoryPath);
    }
}