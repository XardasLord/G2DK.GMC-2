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

		private readonly Stack<ICommand> _executedCommands = new Stack<ICommand>();

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

		public void Run() 
			=> Profile.ExecutionCommands.ForEach(RunSingleCommand);

		public void Revert()
		{
			while (_executedCommands.Count > 0)
			{
				var command = _executedCommands.Pop();

				RevertSingleCommand(command);
			}
		}

		private void RunSingleCommand(ICommand command)
		{
			_executedCommands.Push(command);

			var stopWatch = new Stopwatch();
			stopWatch.Start();
			Logger.StartCommand(command.CommandName);
			
			command.Execute();

			stopWatch.Stop();
			Logger.FinishCommand($"Execution time: {stopWatch.Elapsed}");
		}

		private static void RevertSingleCommand(ICommand command)
		{
			var stopWatch = new Stopwatch();
			stopWatch.Start();
			Logger.StartCommandRevert(command.CommandName);

			command.Revert();

			stopWatch.Stop();
			Logger.FinishCommandRevert($"Execution time: {stopWatch.Elapsed}");
		}
	}
}