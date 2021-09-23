using System;
using System.IO;

namespace GothicModComposer.UI.Interfaces
{
    public interface IZenWorldsFileWatcherService
    {
        void SetWorldsPath(string worldsDirectoryPath);
        void StartWatching(Action<object, FileSystemEventArgs> notifyCallbackSubscription);
        void StopWatching(Action<object, FileSystemEventArgs> notifyCallbackSubscription);
    }
}