using System.Collections.Generic;
using System.IO;
using GothicModComposer.Core.Commands.ExecutedCommandActions;
using GothicModComposer.Core.Commands.ExecutedCommandActions.Interfaces;
using GothicModComposer.Core.Models.Profiles;
using GothicModComposer.Core.Models.VdfFiles;
using GothicModComposer.Core.Utils.IOHelpers;

namespace GothicModComposer.Core.Commands
{
    public class DisableVdfFilesCommand : ICommand
    {
        private static readonly Stack<ICommandActionVDF> ExecutedActions = new();

        private readonly IProfile _profile;

        public DisableVdfFilesCommand(IProfile profile)
            => _profile = profile;

        public string CommandName => "Disable VDF files";

        public async Task ExecuteAsync()
        {
            DirectoryHelper.GetAllFilesInDirectory(_profile.GothicFolder.DataFolderPath, SearchOption.TopDirectoryOnly)
                .ConvertAll(file => new VdfFile(file))
                .FindAll(vdf => vdf.IsEnabled && vdf.IsBaseVdf)
                .ForEach(vdf =>
                {
                    vdf.Disable();

                    ExecutedActions.Push(CommandActionVDF.FileDisabled(vdf));
                });
        }

        public void Undo() => ExecutedActions.Undo();
    }
}