using System.Collections.Generic;
using System.Diagnostics;
using GothicModComposer.Commands;
using GothicModComposer.Models.Folders;
using GothicModComposer.Models.Profiles;
using GothicModComposer.Utils;

namespace GothicModComposer
{
	public class GmcManager
	{
		public GmcFolder GmcFolder { get; }

		private readonly ProfileLoaderResponse _profileLoaderResponse;
		private readonly Stack<ICommand> _executedCommands = new();

		private GmcManager(ProfileLoaderResponse profileLoaderResponse)
		{
			_profileLoaderResponse = profileLoaderResponse;
			GmcFolder = profileLoaderResponse.Profile.GmcFolder;
		}

		public static GmcManager Create(ProfileLoaderResponse profileLoaderResponse)
		{
			return new GmcManager(profileLoaderResponse);
		}

		public void Run() 
			=> _profileLoaderResponse.Commands.ForEach(RunSingleCommand);

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

			// TODO: Introduce general progress bar of the all profile processing (commands processing as childs)

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