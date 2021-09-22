using System.Collections.Generic;
using System.IO;
using GothicModComposer.Commands.ExecutedCommandActions;
using GothicModComposer.Commands.ExecutedCommandActions.Interfaces;
using GothicModComposer.Models;
using GothicModComposer.Models.Profiles;
using GothicModComposer.Utils.IOHelpers;

namespace GothicModComposer.Commands
{
    public class EnableVideoBikFilesCommand : ICommand
    {
        private static readonly Stack<ICommandActionVideoBik> ExecutedActions = new();

        private readonly IProfile _profile;

        public EnableVideoBikFilesCommand(IProfile profile)
            => _profile = profile;

        public string CommandName => "Enable Video BIK files";

        public void Execute()
        {
            DirectoryHelper
                .GetAllFilesInDirectory(_profile.GothicFolder.VideoBikFolderPath, SearchOption.TopDirectoryOnly)
                .ConvertAll(file => new VideoBikFile(file))
                .FindAll(videoFile => videoFile.IsDisabled && videoFile.IsValidVideoBikFile)
                .ForEach(videoFile =>
                {
                    videoFile.Enable();

                    ExecutedActions.Push(CommandActionVideoBik.FileEnabled(videoFile));
                });
        }

        public void Undo() => ExecutedActions.Undo();
    }
}