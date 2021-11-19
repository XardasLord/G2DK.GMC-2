﻿using System.Collections.Generic;
using System.IO;
using GothicModComposer.Core.Commands.ExecutedCommandActions;
using GothicModComposer.Core.Commands.ExecutedCommandActions.Interfaces;
using GothicModComposer.Core.Models;
using GothicModComposer.Core.Models.Profiles;
using GothicModComposer.Core.Utils.IOHelpers;

namespace GothicModComposer.Core.Commands
{
    public class DisableVideoBikFilesCommand : ICommand
    {
        private static readonly Stack<ICommandActionVideoBik> ExecutedActions = new();

        private readonly IProfile _profile;

        public DisableVideoBikFilesCommand(IProfile profile)
            => _profile = profile;

        public string CommandName => "Disable Logo Video BIK files";

        public async Task ExecuteAsync()
        {
            DirectoryHelper
                .GetAllFilesInDirectory(_profile.GothicFolder.VideoBikFolderPath, SearchOption.TopDirectoryOnly)
                .ConvertAll(file => new VideoBikFile(file))
                .FindAll(videoFile => videoFile.IsEnabled && videoFile.IsValidVideoBikFile && videoFile.IsLogoVideo)
                .ForEach(videoFile =>
                {
                    videoFile.Disable();

                    ExecutedActions.Push(CommandActionVideoBik.FileDisabled(videoFile));
                });
        }

        public void Undo() => ExecutedActions.Undo();
    }
}