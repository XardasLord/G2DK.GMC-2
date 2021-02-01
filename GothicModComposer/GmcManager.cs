using System.Collections.Generic;
using System.Diagnostics;
using GothicModComposer.Commands;
using GothicModComposer.Models;
using GothicModComposer.Utils;

namespace GothicModComposer
{
	public class GmcManager
	{
		public GothicFolder GothicFolder { get; }
		public GmcFolder GmcFolder { get; }
		public ModFolder ModFolder { get; }
		public ProfileDefinition Profile { get; }

		private readonly Stack<ICommand> _executedCommands = new();

		private GmcManager(GothicFolder gothicFolder, GmcFolder gmcFolder, ModFolder modFolder, ProfileDefinition profile)
		{
			GothicFolder = gothicFolder;
			GmcFolder = gmcFolder;
			ModFolder = modFolder;
			Profile = profile;
		}

		public static GmcManager Create(GothicFolder gothicFolder, GmcFolder gmcFolder, ModFolder modFolder, ProfileDefinition profile)
		{
			return new GmcManager(gothicFolder, gmcFolder, modFolder, profile);
		}

		public void Run() 
			=> Profile.ExecutionCommands.ForEach(RunSingleCommand);

		public void Undo()
		{
			while (_executedCommands.Count > 0)
			{
				var command = _executedCommands.Pop();

				UndoSingleCommand(command);
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

		private static void UndoSingleCommand(ICommand command)
		{
			var stopWatch = new Stopwatch();
			stopWatch.Start();
			Logger.StartCommandUndo(command.CommandName);

			command.Undo();

			stopWatch.Stop();
			Logger.FinishCommandUndo($"Execution time: {stopWatch.Elapsed}");
		}
	}
}