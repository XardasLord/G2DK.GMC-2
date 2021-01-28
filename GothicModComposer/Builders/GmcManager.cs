using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using GothicModComposer.Commands;
using GothicModComposer.Models;
using GothicModComposer.Utils;

namespace GothicModComposer.Builders
{
	public class GmcManager
	{
		public GothicFolder GothicFolder { get; }
		public GmcFolder GmcFolder { get; }
		public ProfileDefinition Profile { get; }

		private GmcManager(GothicFolder gothicFolder, GmcFolder gmcFolder, ProfileDefinition profile)
		{
			GothicFolder = gothicFolder;
			GmcFolder = gmcFolder;
			Profile = profile;
		}

		public static GmcManager Create(GothicFolder gothicFolder, GmcFolder gmcFolder, ProfileDefinition profile)
		{
			return new GmcManager(gothicFolder, gmcFolder, profile);
		}

		public Task Run()
		{
			Profile.ExecutionCommands.ForEach(async command => await RunSingleCommand(command));

			return Task.CompletedTask;
		}

		private static async Task RunSingleCommand(ICommand command)
		{
			var stopWatch = new Stopwatch();
			stopWatch.Start();
			Logger.StartCommand(command.CommandName);

			await command.Execute();

			stopWatch.Stop();
			Logger.FinishCommand($"Execution time: {stopWatch.Elapsed}");
		}
	}
}