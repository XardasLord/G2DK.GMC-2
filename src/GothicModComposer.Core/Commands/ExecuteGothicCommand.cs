using System.Diagnostics;
using System.IO;
using GothicModComposer.Core.Models.Profiles;
using GothicModComposer.Core.Utils;
using GothicModComposer.Core.Utils.GothicSpyProcess;
using GothicModComposer.Core.Utils.ProgressBar;
using ShellProgressBar;

namespace GothicModComposer.Core.Commands
{
    public class ExecuteGothicCommand : ICommand
    {
        public const string WorldLoadedMessage =
            "Info:  1 B:     GMAN: Completed loading the world ... .... <oGameManager.cpp,#1476>";

        public const string MainMenuOpenMessage =
            "Info:  4 B:     GMAN: Open InitScreen .... <oGameManager.cpp,#811>";

        public const string MainMenuClosedMessage =
            "Info:  4 B:     GMAN: Close InitScreen .... <oGameManager.cpp,#849>";

        public const string LoadingGothicSrcOrGothicDat =
            "Info:  4 N:     GAME: Loading file Content\\Gothic.src or .dat .... <oGame.cpp,#739>";

        private readonly GothicSpyProcessRunner _gothicSpyProcessRunner;
        private readonly string _killProcessMessage;

        private readonly IProfile _profile;
        private Process _gothicProcess;

        private IndeterminateProgressBar _rootProgressBar;
        private ChildProgressBar _textureCompilationProgressBar;
        private ChildProgressBar _meshesCompilationProgressBar;

        private FileSystemWatcher _compiledTexturesFileWatcher;
        private FileSystemWatcher _compiledMeshesFileWatcher;

        public ExecuteGothicCommand(IProfile profile, string killProcessMessage = null)
        {
            _profile = profile;
            _killProcessMessage = killProcessMessage;
            _gothicSpyProcessRunner = new GothicSpyProcessRunner();
        }

        public string CommandName => "Execute Gothic2.exe";

        public void Execute()
        {
            if (!_profile.CommandsConditions.ExecuteGothicStepRequired)
            {
                Logger.Info("Gothic compilation is not required, so this step can be skipped.", true);
                return;
            }

            Logger.Info($"Executing with kill process message: '{_killProcessMessage}'", true);

            _gothicProcess = GetGothicProcess();

            _gothicSpyProcessRunner.Run();
            _gothicSpyProcessRunner.Subscribe(Notify);

            Logger.Info($"{_gothicProcess.StartInfo.FileName} {_gothicProcess.StartInfo.Arguments}", true);

            using (_rootProgressBar = new IndeterminateProgressBar(
                $"Gothic2.exe process executed with arguments '{_gothicProcess.StartInfo.Arguments}'",
                ProgressBarOptionsHelper.Get()))
            {
                if (IsTextureCompilationRequired())
                    StartRealTimeProgressOnTextureCompilation();

                if (IsMeshesCompilationRequired())
                    StartRealTimeProgressOnMeshesCompilation();

                _gothicProcess.Start();
                _gothicProcess.WaitForExit();

                _rootProgressBar.Finished();

                _textureCompilationProgressBar?.Dispose();
                _meshesCompilationProgressBar?.Dispose();

                _compiledTexturesFileWatcher?.Dispose();
                _compiledMeshesFileWatcher?.Dispose();
            }

            _gothicSpyProcessRunner.Abort();
        }

        public void Undo() => Logger.Warn("Undo for this command is not implemented yet.");

        private void Notify(string message)
        {
            Logger.zLog(message);

            if (_rootProgressBar != null)
                _rootProgressBar.Message = message;

            if (_killProcessMessage is null || !message.Contains(_killProcessMessage))
                return;

            _gothicProcess.Kill();

            Logger.Info("Gothic process was terminated.", true);
        }

        private Process GetGothicProcess()
        {
            string arguments;

            if (_killProcessMessage is null)
                // If we run the game then we need to merge configs from UI
                arguments = _profile.GothicArguments.Merge(_profile.GothicArgumentsForceConfig).ToString();
            else
                arguments = _profile.GothicArguments.ToString();

            return new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = _profile.GothicFolder.GothicExeFilePath,
                    WorkingDirectory = _profile.GothicFolder.BasePath,
                    Arguments = arguments,
                    Verb = "runas", // Force to run the process as Administrator
                    UseShellExecute = false
                }
            };
        }

        private bool IsTextureCompilationRequired()
            => _profile.GothicArguments.Contains(GothicArguments.ZConvertAllParameter) ||
               _profile.GothicArguments.Contains(GothicArguments.ZTexConvertParameter);

        private bool IsMeshesCompilationRequired() 
            => _profile.GothicArguments.Contains(GothicArguments.ZConvertAllParameter);

        private void StartRealTimeProgressOnTextureCompilation()
        {
            var numberOfTexturesToCompile = _profile.GothicFolder.GetNumberOfTexturesToCompile();
            var counter = 1;

            _textureCompilationProgressBar = _rootProgressBar?.Spawn(
                numberOfTexturesToCompile, "Compiling textures", ProgressBarOptionsHelper.Get());

            _compiledTexturesFileWatcher = new FileSystemWatcher(_profile.GothicFolder.CompiledTexturesPath);
            _compiledTexturesFileWatcher.Created += (_, _) =>
            {
                _textureCompilationProgressBar?.Tick(
                    $"Compiled {counter++} of {numberOfTexturesToCompile} textures");
            };

            _compiledTexturesFileWatcher.EnableRaisingEvents = true;
        }

        private void StartRealTimeProgressOnMeshesCompilation()
        {
            var numberOfMeshesToCompile = _profile.GothicFolder.GetNumberOfMeshesToCompile();
            var counter = 1;

            _meshesCompilationProgressBar = _rootProgressBar?.Spawn(
                numberOfMeshesToCompile, "Compiling meshes", ProgressBarOptionsHelper.Get());

            _compiledMeshesFileWatcher = new FileSystemWatcher(_profile.GothicFolder.CompiledMeshesPath);
            _compiledMeshesFileWatcher.Created += (_, _) =>
            {
                _meshesCompilationProgressBar?.Tick($"Compiled {counter++} of {numberOfMeshesToCompile} meshes");
            };

            _compiledMeshesFileWatcher.EnableRaisingEvents = true;
        }
    }
}