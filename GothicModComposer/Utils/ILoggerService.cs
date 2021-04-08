namespace GothicModComposer.Utils
{
    public interface ILoggerService
    {
        void Info(string message, bool display = false);
        void Warn(string message);
        void Error(string message);
        void ZLog(string message);
        void StartCommand(string message);
        void StartCommandUndo(string message);
        void FinishCommand(string message);
        void FinishCommandUndo(string message);
        void SaveLogs(string logsFolderPath);
    }
}