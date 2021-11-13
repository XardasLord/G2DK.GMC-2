using System.Collections.Generic;
using System.Diagnostics;
using GothicModComposer.Core.Commands;
using GothicModComposer.Core.Models.Interfaces;
using GothicModComposer.Core.Models.Profiles;
using GothicModComposer.Core.Utils;

namespace GothicModComposer.Core
{
    public class GmcManager
    {
        private readonly Stack<ICommand> _executedCommands = new();

        private readonly ProfileLoaderResponse _profileLoaderResponse;

        private GmcManager(ProfileLoaderResponse profileLoaderResponse)
        {
            _profileLoaderResponse = profileLoaderResponse;
            GmcFolder = profileLoaderResponse.Profile.GmcFolder;
        }

        public IGmcFolder GmcFolder { get; }

        public static GmcManager Create(ProfileLoaderResponse profileLoaderResponse) =>
            new GmcManager(profileLoaderResponse);

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

            // TODO: Introduce general progress bar of the all profile processing (commands processing as child)

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