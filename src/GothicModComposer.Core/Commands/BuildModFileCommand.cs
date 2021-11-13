using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using GothicModComposer.Core.Models.Profiles;
using GothicModComposer.Core.Utils;
using GothicModComposer.Core.Utils.IOHelpers;
using GothicModComposer.Core.Utils.ProgressBar;

namespace GothicModComposer.Core.Commands
{
    public class BuildModFileCommand : ICommand
    {
        private readonly IProfile _profile;

        public BuildModFileCommand(IProfile profile)
            => _profile = profile;

        public string CommandName => "Build .MOD file";

        public void Execute()
        {
            CreateVdfsConfigFile();
            ExecuteGothicVdfsProcess();
            CopyIniFile();
        }

        // TODO: Implement undo mechanism
        public void Undo() => throw new NotImplementedException();

        private void CreateVdfsConfigFile()
        {
            var vdfsConfig = _profile.GothicVdfsConfig;

            DirectoryHelper.CreateIfDoesNotExist(_profile.GmcFolder.BuildFolderPath);

            var configContent = GothicVdfsConfigWriter.GenerateContent(vdfsConfig, _profile.GothicFolder.BasePath,
                _profile.GmcFolder.BuildFolderPath);
            FileHelper.SaveContent(_profile.GmcFolder.VdsfConfigFilePath, configContent, Encoding.Default);

            Logger.Info("Generated content of VDFS config file:", true);
            Logger.Info($"\n{configContent}", true);
        }

        private void ExecuteGothicVdfsProcess()
        {
            var vdsfProcessStartInfo = new ProcessStartInfo
            {
                FileName = _profile.GothicFolder.GothicVdfsToolFilePath,
                WorkingDirectory = _profile.GothicFolder.BasePath,
                UseShellExecute = false,
                CreateNoWindow = true,
                Arguments = $"/B \"{_profile.GmcFolder.VdsfConfigFilePath}\""
            };

            using var progressBar =
                new IndeterminateProgressBar(".MOD file generating process...", ProgressBarOptionsHelper.Get());

            Logger.Info($"{_profile.GothicFolder.GothicVdfsToolFilePath} {vdsfProcessStartInfo.Arguments}", true);

            var vdfsProcess = new Process {StartInfo = vdsfProcessStartInfo};

            vdfsProcess.Start();
            vdfsProcess.WaitForExit();

            progressBar.Finished();
        }

        private void CopyIniFile()
        {
            var files = DirectoryHelper.GetAllFilesInDirectory(_profile.ModFolder.BasePath);
            var modIniFile = files.Find(item => item.Contains(".ini"));

            if (string.IsNullOrEmpty(modIniFile))
                return;

            var dest = Path.Combine(_profile.GmcFolder.BuildFolderPath, Path.GetFileName(modIniFile));
            FileHelper.CopyWithOverwrite(modIniFile, dest);
        }
    }
}