using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using GothicModComposer.Core.Commands;
using GothicModComposer.Core.Models.Interfaces;
using GothicModComposer.Core.Models.Profiles;
using GothicModComposer.Core.Utils;

namespace GothicModComposer.Core
{
    public class GmcCoreManager
    {
        private readonly Stack<ICommand> _executedCommands = new();

        private readonly ProfileLoaderResponse _profileLoaderResponse;

        private readonly IGmcFolder _gmcFolder;

        private GmcCoreManager(ProfileLoaderResponse profileLoaderResponse)
        {
            _profileLoaderResponse = profileLoaderResponse;
            _gmcFolder = profileLoaderResponse.Profile.GmcFolder;
        }

        public static GmcCoreManager Create(ProfileLoaderResponse profileLoaderResponse) =>
            new(profileLoaderResponse);

        public void Run()
        {
            var fullVersion = Assembly.GetExecutingAssembly().GetName().Version;
            var stopWatch = new Stopwatch();
            
            try
            {
                stopWatch.Start();
                Logger.Info($"GMC v{fullVersion?.Major}.{fullVersion?.Minor}.{fullVersion?.Build}", true);
                Logger.Info($"GMC build with profile {_profileLoaderResponse.Profile.ProfileType} started...", true);

                _profileLoaderResponse.Commands.ForEach(RunSingleCommand);

                stopWatch.Stop();
                Logger.Info($"GMC build finished. Execution time: {stopWatch.Elapsed}", true);
                Logger.Info("You can close the application.", true);
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);

                Console.WriteLine("Press any key to start the undo changes process...");
                Console.ReadKey();

                stopWatch.Restart();
                Logger.Info("GMC undo changes started...", true);

                Undo();

                stopWatch.Stop();
                Logger.Info($"GMC undo changes finished. Execution time: {stopWatch.Elapsed}", true);
                Logger.Info("You can close the application.", true);
            }
            finally
            {
                Logger.SaveLogs();
            }
        }

        private void Undo()
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