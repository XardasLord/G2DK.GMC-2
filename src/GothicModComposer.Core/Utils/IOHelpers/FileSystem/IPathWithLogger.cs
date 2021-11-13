namespace GothicModComposer.Core.Utils.IOHelpers.FileSystem
{
    public interface IPathWithLogger
    {
        IFileSystemWithLogger FileSystem { get; }

        string Combine(params string[] paths);

        string Combine(string path1, string path2);

        string Combine(string path1, string path2, string path3);

        string Combine(string path1, string path2, string path3, string path4);

        string GetDirectoryName(string path);

        string GetExtension(string path);

        string GetFileName(string path);

        string GetFileNameWithoutExtension(string path);

        string GetFullPath(string path);

        string GetFullPath(string path, string basePath);

        string GetPathRoot(string path);

        string GetRelativePath(string relativeTo, string path);
    }
}