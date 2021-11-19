﻿using GothicModComposer.Core.Models.Profiles;
using GothicModComposer.Core.Utils;

namespace GothicModComposer.Core.Commands
{
    public class ClearGmcTemporaryFilesCommand : ICommand
    {
        private readonly IProfile _profile;

        public ClearGmcTemporaryFilesCommand(IProfile profile)
            => _profile = profile;

        public string CommandName => "Clear GMC temporary files";

        public async Task ExecuteAsync()
        {
            _profile.GmcFolder.DeleteTemporaryFiles();
            _profile.GothicFolder.DeleteGmcIni();
        }

        public void Undo() => Logger.Info("Undo of this command is not available.", true);
    }
}