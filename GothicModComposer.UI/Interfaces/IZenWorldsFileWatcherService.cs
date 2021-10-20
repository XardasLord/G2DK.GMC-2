using System;
using System.IO;

namespace GothicModComposer.UI.Interfaces
{
    public interface IZenWorldsFileWatcherService
    {
        void SetHandlers(Action<object, FileSystemEventArgs> notifyCallbackSubscription);
        void SetWorldsPath(string worldsDirectoryPath);
        void StartWatching();
        void StopWatching();
    }
}