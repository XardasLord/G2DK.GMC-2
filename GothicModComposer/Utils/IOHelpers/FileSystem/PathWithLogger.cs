using System.IO;

namespace GothicModComposer.Utils.IOHelpers.FileSystem
{
    public class PathWithLogger : IPathWithLogger
    {
        public IFileSystemWithLogger FileSystem { get; }

        public PathWithLogger(IFileSystemWithLogger fileSystem)
            => FileSystem = fileSystem;

        public string Combine(params string[] paths) => Path.Combine(paths);

        public string Combine(string path1, string path2) => Path.Combine(path1, path2);

        public string Combine(string path1, string path2, string path3) => Path.Combine(path1, path2, path3);

        public string Combine(string path1, string path2, string path3, string path4) => Path.Combine(path1, path2, path3, path4);

        public string GetDirectoryName(string path) => Path.GetDirectoryName(path);

        public string GetExtension(string path) => Path.GetExtension(path);

        public string GetFileName(string path) => Path.GetFileName(path);

        public string GetFileNameWithoutExtension(string path) => Path.GetFileNameWithoutExtension(path);

        public string GetFullPath(string path) => Path.GetFullPath(path);

        public string GetFullPath(string path, string basePath) => Path.GetFullPath(path, basePath);

        public string GetPathRoot(string path) => Path.GetPathRoot(path);

        public string GetRelativePath(string relativeTo, string path) => Path.GetRelativePath(relativeTo, path);
    }
}