using System.IO;
using GothicModComposer.UI.Interfaces;

namespace GothicModComposer.UI.Services
{
    public class FileService : IFileService
    {
        public bool HasBinaryContent(string filePath)
        {
            var fileInfo = new FileInfo(filePath);

            if (IsFileLocked(fileInfo))
            {
                return false;
            }
            
            if (!File.Exists(filePath)) 
                return false;
        
            var content = File.ReadAllBytes(filePath);
            
            for (var i = 1; i < 512 && i < content.Length; i++) {
                // Is it binary? Check for consecutive nulls..
                if (content[i] == 0x00 && content[i-1] == 0x00)
                {
                    return true;
                }
            }
            
            return false;
        }
        
        private static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                // the file is unavailable because it is:
                // still being written to
                // or being processed by another thread
                // or does not exist (has already been processed)
                return true;
            }
            finally
            {
                stream?.Close();
            }

            // file is not locked
            return false;
        }
    }
}