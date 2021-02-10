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
			Parser.Default.ParseArguments<GmcInitialParameter>(args)
				.WithParsed(RunGmc);

			Console.ReadKey();
		}

		private static void RunGmc(GmcInitialParameter parameters)
		{
			var stopWatch = new Stopwatch();

			var gmcManager = GmcManagerBuilder.PrepareGmcExecutor(parameters.Profile, parameters.AbsolutePathToProject);

			try
			{
				stopWatch.Start();
				Logger.Info($"GMC build with profile {parameters.Profile} started...", true);

				gmcManager.Run();

				stopWatch.Stop();
				Logger.Info($"GMC build finished. Execution time: {stopWatch.Elapsed}", true);
			}
			catch (Exception e)
			{
				Logger.Error(e.Message);

				Console.WriteLine("Press any key to start the undo changes process...");
				Console.ReadKey();

				stopWatch.Restart();
				Logger.Info("GMC undo changes started...", true);

				gmcManager.Undo();

				stopWatch.Stop();
				Logger.Info($"GMC undo changes finished. Execution time: {stopWatch.Elapsed}", true);
			}
			finally
			{
				Logger.SaveLogs(gmcManager.GmcFolder.BasePath);
			}
		}
	}
}
