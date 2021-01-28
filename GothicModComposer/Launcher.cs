using System;
using System.Diagnostics;
using System.Threading.Tasks;
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
				.WithParsedAsync(RunGmc);

			Console.ReadKey();
		}

		private static async Task RunGmc(InitialParameter parameters)
		{
			var stopWatch = new Stopwatch();
			stopWatch.Start();
			Logger.Info($"GMC build with profile {parameters.Profile} started...");

			await GmcExecutorBuilder
				.PrepareGmcExecutor(parameters.Profile, parameters.AbsolutePathToProject)
				.Run();

			stopWatch.Stop();
			Logger.Info($"GMC build execution time: {stopWatch.Elapsed}");
		}
	}
}
