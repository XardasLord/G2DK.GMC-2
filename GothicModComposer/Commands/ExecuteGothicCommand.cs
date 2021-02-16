using System;
using System.Diagnostics;
using GothicModComposer.Models.Profiles;
using GothicModComposer.Utils;
using GothicModComposer.Utils.GothicSpyProcess;

namespace GothicModComposer.Commands
{
	public class ExecuteGothicCommand : ICommand
	{
		public string CommandName => "Execute Gothic";

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

		public ExecuteGothicCommand(IProfile profile, string killProcessMessage = null)
		{
			_profile = profile;
			_killProcessMessage = killProcessMessage;
			_gothicSpyProcessRunner = new GothicSpyProcessRunner();
		}

		public void Execute()
		{
			Logger.Info($"Executing with kill process message: '{_killProcessMessage}'", true);

			_gothicProcess = new Process
			{
				StartInfo = GetGothicProcessStartInfo()
			};

			_gothicSpyProcessRunner.Run();
			_gothicSpyProcessRunner.Subscribe(Notify);

			Logger.Info($"{_gothicProcess.StartInfo.FileName} {_gothicProcess.StartInfo.Arguments}", true);

			_gothicProcess.Start();
			_gothicProcess.WaitForExit();

			_gothicSpyProcessRunner.Abort();
		}

		public void Undo()
		{
			throw new NotImplementedException();
		}

		private void Notify(string message)
		{
			Logger.zLog(message);

			if (_killProcessMessage == null || !message.Contains(_killProcessMessage))
				return;

			_gothicProcess.Kill();

			Logger.Info("Gothic process was terminated.", true);
		}

		private ProcessStartInfo GetGothicProcessStartInfo()
			=> new()
			{
				FileName = _profile.GothicFolder.GothicExeFilePath,
				WorkingDirectory = _profile.GothicFolder.BasePath,
				Arguments = _profile.GothicArguments.ToString(),
				Verb = "runas", // Force to run the process as Adminitrator
				UseShellExecute = false
			};
	}
}
