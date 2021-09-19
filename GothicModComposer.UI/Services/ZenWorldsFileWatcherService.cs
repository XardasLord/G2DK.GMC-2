using System;
using System.IO;
using GothicModComposer.UI.Interfaces;

namespace GothicModComposer.UI.Services
{
    public class ZenWorldsFileWatcherService : IZenWorldsFileWatcherService
    {
        private readonly FileSystemWatcher _zenWorldsFileWatcher;

        public ZenWorldsFileWatcherService()
        {
            _zenWorldsFileWatcher = new FileSystemWatcher();
            _zenWorldsFileWatcher.IncludeSubdirectories = true;
        }

        public void SetWorldsPath(string worldsDirectoryPath)
        {
            _zenWorldsFileWatcher.Path = worldsDirectoryPath;
        }

        public void StartWatching(Action<object, FileSystemEventArgs> notifyCallbackSubscription)
        {
            _zenWorldsFileWatcher.Created += notifyCallbackSubscription.Invoke;
            _zenWorldsFileWatcher.Renamed += notifyCallbackSubscription.Invoke;
            _zenWorldsFileWatcher.Deleted += notifyCallbackSubscription.Invoke;

            _zenWorldsFileWatcher.EnableRaisingEvents = true;
        }

        public void StopWatching(Action<object, FileSystemEventArgs> notifyCallbackSubscription)
        {
            _zenWorldsFileWatcher.Created -= notifyCallbackSubscription.Invoke;
            _zenWorldsFileWatcher.Renamed -= notifyCallbackSubscription.Invoke;
            _zenWorldsFileWatcher.Deleted -= notifyCallbackSubscription.Invoke;

            _zenWorldsFileWatcher.EnableRaisingEvents = false;
        }
    }
}