using System.IO.Abstractions;

namespace GothicModComposer.Utils.IOHelpers.FileSystem
{
    public interface IFileSystemWithLogger
    {
        IFileWithLogger File { get; }

        IDirectoryWithLogger Directory { get; }

        IFileInfoFactory FileInfo { get; }

        IFileStreamFactory FileStream { get; }

        IPathWithLogger Path { get; }

        IDirectoryInfoFactory DirectoryInfo { get; }

        IDriveInfoFactory DriveInfo { get; }

        IFileSystemWatcherFactory FileSystemWatcher { get; }
    }
}