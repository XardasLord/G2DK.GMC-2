using System;
using System.Diagnostics;
using CommandLine;
using GothicModComposer.Builders;
using GothicModComposer.Models;
using GothicModComposer.Utils;

namespace GothicModComposer
{
	internal class Launcher
	{
		private static void Main(string[] args)
		{
			Parser.Default.ParseArguments<InitialParameter>(args)
				.WithParsed(RunGmc);

			Console.ReadKey();
		}

		private static void RunGmc(InitialParameter parameters)
		{
			var stopWatch = new Stopwatch();

			var gmcManager = GmcManagerBuilder.PrepareGmcExecutor(parameters.Profile, parameters.AbsolutePathToProject);

			try
			{
				stopWatch.Start();
				Logger.Info($"GMC build with profile {parameters.Profile} started...");

				gmcManager.Run();

				stopWatch.Stop();
				Logger.Info($"GMC build finished. Execution time: {stopWatch.Elapsed}");
			}
			catch (Exception e)
			{
				Logger.Error(e.Message);
				
				stopWatch.Restart();
				Logger.Info("GMC undo changes started...");

				gmcManager.Undo();

				stopWatch.Stop();
				Logger.Info($"GMC undo changes finished. Execution time: {stopWatch.Elapsed}");
			}

		}
	}
}
