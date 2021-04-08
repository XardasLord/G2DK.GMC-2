using GothicModComposer.Utils.IOHelpers;

namespace GothicModComposer.Utils
{
    public class LoggerService : ILoggerService
    {
        public void Info(string message, bool display = false) => Logger.Info(message, display);

        public void Warn(string message) => Logger.Warn(message); 

        public void Error(string message) => Logger.Error(message);

        public void ZLog(string message) => Logger.zLog(message);

        public void StartCommand(string message) => Logger.StartCommand(message);

        public void StartCommandUndo(string message) => Logger.StartCommandUndo(message);

        public void FinishCommand(string message) => Logger.FinishCommand(message);

        public void FinishCommandUndo(string message) => Logger.FinishCommandUndo(message);

        public void SaveLogs(string logsFolderPath) => Logger.SaveLogs(logsFolderPath);
    }
}