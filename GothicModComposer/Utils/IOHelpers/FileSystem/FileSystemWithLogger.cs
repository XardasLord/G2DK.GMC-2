using System.IO.Abstractions;

namespace GothicModComposer.Utils.IOHelpers.FileSystem
{
    public class FileSystemWithLogger : IFileSystemWithLogger
    {
        public FileSystemWithLogger()
        {
            DriveInfo = null;
            DirectoryInfo = null;
            FileInfo = null;
            Path = new PathWithLogger(this);
            File = new FileWithLogger(this);
            Directory = new DirectoryWithLogger(this);
            FileStream = null;
            FileSystemWatcher = null;
        }

        public IFileWithLogger File { get; }
        public IDirectoryWithLogger Directory { get; }
        public IFileInfoFactory FileInfo { get; }
        public IFileStreamFactory FileStream { get; }
        public IPathWithLogger Path { get; }
        public IDirectoryInfoFactory DirectoryInfo { get; }
        public IDriveInfoFactory DriveInfo { get; }
        public IFileSystemWatcherFactory FileSystemWatcher { get; }
    }
}