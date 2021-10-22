using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using GothicModComposer.Commands.ExecutedCommandActions;
using GothicModComposer.Commands.ExecutedCommandActions.Interfaces;
using GothicModComposer.Models;
using GothicModComposer.Models.Profiles;
using GothicModComposer.Utils.IOHelpers;

namespace GothicModComposer.Commands
{
    public class DisableVideoBikFilesCommand : ICommand
    {
        private static readonly ConcurrentStack<ICommandActionVideoBik> ExecutedActions = new();
        private static object _lock = new();
        private readonly IProfile _profile;

        public DisableVideoBikFilesCommand(IProfile profile)
            => _profile = profile;

        public string CommandName => "Disable Logo Video BIK files";

        public void Execute()
        {
            var files = DirectoryHelper
                .GetAllFilesInDirectory(_profile.GothicFolder.VideoBikFolderPath, SearchOption.TopDirectoryOnly)
                .ConvertAll(file => new VideoBikFile(file))
                .FindAll(videoFile => videoFile.IsEnabled && videoFile.IsValidVideoBikFile && videoFile.IsLogoVideo);

            Parallel.ForEach(files, videoFile =>
            {
                videoFile.Disable();
                ExecutedActions.Push(CommandActionVideoBik.FileDisabled(videoFile));
            });
        }

        public void Undo() => ExecutedActions.Undo();
    }
}