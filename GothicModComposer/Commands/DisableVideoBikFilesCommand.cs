using System.Collections.Generic;
using System.IO;
using GothicModComposer.Commands.ExecutedCommandActions;
using GothicModComposer.Commands.ExecutedCommandActions.Interfaces;
using GothicModComposer.Models;
using GothicModComposer.Models.Profiles;
using GothicModComposer.Utils.IOHelpers;

namespace GothicModComposer.Commands
{
    public class DisableVideoBikFilesCommand : ICommand
    {
        public string CommandName => "Disable Video BIK files";

        private readonly IProfile _profile;
        private static readonly Stack<ICommandActionVideoBik> ExecutedActions = new();

        public DisableVideoBikFilesCommand(IProfile profile) 
            => _profile = profile;

        public void Execute()
        {
            DirectoryHelper.GetAllFilesInDirectory(_profile.GothicFolder.VideoBikFolderPath, SearchOption.TopDirectoryOnly)
                .ConvertAll(file => new VideoBikFile(file))
                .FindAll(videoFile => videoFile.IsEnabled && videoFile.IsValidVideoBikFile)
                .ForEach(videoFile =>
                {
                    videoFile.Disable();
					
                    ExecutedActions.Push(CommandActionVideoBik.FileDisabled(videoFile));
                });
        }

        public void Undo() => ExecutedActions.Undo();
    }
}