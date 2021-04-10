namespace GothicModComposer.Utils.IOHelpers.FileSystem
{
    public class FileSystemWithLogger : IFileSystemWithLogger
    {
        public FileSystemWithLogger()
        {
            Path = new PathWithLogger(this);
            File = new FileWithLogger(this);
            Directory = new DirectoryWithLogger(this);
        }

        public IFileWithLogger File { get; }
        public IDirectoryWithLogger Directory { get; }
        public IPathWithLogger Path { get; }
    }
}