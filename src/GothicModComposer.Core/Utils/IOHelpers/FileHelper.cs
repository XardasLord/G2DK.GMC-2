using System;
using System.IO;
using System.Text;

namespace GothicModComposer.Core.Utils.IOHelpers
{
    public static class FileHelper
    {
        public static bool Exists(string path)
            => File.Exists(path);

        public static string ReadFile(string path)
            => File.ReadAllText(path);

        public static long GetFileTimestamp(string filePath)
        {
            if (!Exists(filePath))
                return 0;

            var file = new FileInfo(filePath);
            return new DateTimeOffset(file.LastWriteTimeUtc).ToUnixTimeSeconds();
        }

        public static void Copy(string source, string dest)
        {
            CreateMissingDirectoriesIfNotExist(dest);
            File.Copy(source, dest);

            Logger.Info($"Copied file \"{source}\" ---> \"{dest}\".");
        }

        public static void CopyWithOverwrite(string source, string dest)
        {
            if (File.Exists(dest))
            {
                File.Copy(source, dest, true);
            }
            else
            {
                CreateMissingDirectoriesIfNotExist(dest);
                File.Copy(source, dest, true);
            }

            Logger.Info($"Copied file \"{source}\" ---> \"{dest}\".");
        }

        public static void Move(string source, string dest)
        {
            File.Move(source, dest);

            Logger.Info($"Moved file \"{source}\" ---> \"{dest}\".");
        }

        public static void MoveWithOverwrite(string source, string dest)
        {
            if (File.Exists(dest))
            {
                File.Move(source, dest, true);
            }
            else
            {
                CreateMissingDirectoriesIfNotExist(dest);
                File.Move(source, dest, true);
            }

            Logger.Info($"Moved file \"{source}\" ---> \"{dest}\".");
        }

        public static void Rename(string oldNamePath, string newNamePath)
        {
            File.Move(oldNamePath, newNamePath);

            Logger.Info($"Renamed file \"{oldNamePath}\" ---> \"{newNamePath}\".");
        }

        public static bool DeleteIfExists(string path)
        {
            if (!File.Exists(path))
                return false;

            File.Delete(path);

            Logger.Info($"Deleted file \"{path}\".");
            return true;
        }

        public static void SaveContent(string path, string content, Encoding encoding)
            => File.WriteAllText(path, content, encoding);

        private static void CreateMissingDirectoriesIfNotExist(string dest)
        {
            var directoryInfo = new FileInfo(dest).Directory;
            if (directoryInfo != null && directoryInfo.Exists == false) directoryInfo.Create();
        }
    }
}