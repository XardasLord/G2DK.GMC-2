namespace GothicModComposer.Utils.IOHelpers.FileSystem
{
    public interface IFileSystemWithLogger
    {
        IFileWithLogger File { get; }

        IDirectoryWithLogger Directory { get; }

        IPathWithLogger Path { get; }
    }
}