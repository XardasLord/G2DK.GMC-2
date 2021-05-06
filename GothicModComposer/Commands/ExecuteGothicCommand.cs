using System.Diagnostics;
using GothicModComposer.Models.Profiles;
using GothicModComposer.Utils;
using GothicModComposer.Utils.GothicSpyProcess;
using GothicModComposer.Utils.ProgressBar;
using ShellProgressBar;

namespace GothicModComposer.Commands
{
	public class ExecuteGothicCommand : ICommand
	{
		public string CommandName => "Execute Gothic2.exe";

		public const string WorldLoadedMessage =
			"Info:  1 B:     GMAN: Completed loading the world ... .... <oGameManager.cpp,#1476>";
		public const string MainMenuOpenMessage =
			"Info:  4 B:     GMAN: Open InitScreen .... <oGameManager.cpp,#811>";
		public const string MainMenuClosedMessage =
			"Info:  4 B:     GMAN: Close InitScreen .... <oGameManager.cpp,#849>";
		public const string LoadingGothicSrcOrGothicDat =
			"Info:  4 N:     GAME: Loading file Content\\Gothic.src or .dat .... <oGame.cpp,#739>";

		private readonly IProfile _profile;
		private readonly string _killProcessMessage;
		private Process _gothicProcess;
		private readonly GothicSpyProcessRunner _gothicSpyProcessRunner;
		private IndeterminateProgressBar _progressBar;

		public ExecuteGothicCommand(IProfile profile, string killProcessMessage = null)
		{
			_profile = profile;
			_killProcessMessage = killProcessMessage;
			_gothicSpyProcessRunner = new GothicSpyProcessRunner();
		}

		public void Execute()
		{
            if (!_profile.CommandsConditions.ExecuteGothicStepRequired)
            {
                Logger.Info("Gothic compilation is not required, so this step can be skipped.", true);
                return;
			}

			Logger.Info($"Executing with kill process message: '{_killProcessMessage}'", true);

			_gothicProcess = new Process
			{
				StartInfo = GetGothicProcessStartInfo()
			};

			_gothicSpyProcessRunner.Run();
			_gothicSpyProcessRunner.Subscribe(Notify);

			Logger.Info($"{_gothicProcess.StartInfo.FileName} {_gothicProcess.StartInfo.Arguments}", true);

			using (_progressBar = new IndeterminateProgressBar($"Gothic2.exe process executed with arguments '{_gothicProcess.StartInfo.Arguments}'", ProgressBarOptionsHelper.Get()))
			{
				_gothicProcess.Start();
				_gothicProcess.WaitForExit();

				_progressBar.Finished();
			}

			_gothicSpyProcessRunner.Abort();
		}

		public void Undo() => Logger.Warn("Undo for this command is not implemented yet.");

		private void Notify(string message)
		{
			Logger.zLog(message);

			if (_progressBar != null)
				_progressBar.Message = message;

			if (_killProcessMessage is null || !message.Contains(_killProcessMessage))
				return;

			_gothicProcess.Kill();

			Logger.Info("Gothic process was terminated.", true);
		}

		private ProcessStartInfo GetGothicProcessStartInfo()
        {
            var arguments = _profile.GothicArguments.Merge(_profile.GothicArgumentsForceConfig);

            return new ProcessStartInfo
            {
                FileName = _profile.GothicFolder.GothicExeFilePath,
                WorkingDirectory = _profile.GothicFolder.BasePath,
                Arguments = arguments.ToString(),
                Verb = "runas", // Force to run the process as Administrator
                UseShellExecute = false
            };
        }
    }
}
