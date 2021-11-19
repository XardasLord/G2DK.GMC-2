using System.Collections.Generic;
using System.IO;
using GothicModComposer.Core.Commands.ExecutedCommandActions;
using GothicModComposer.Core.Commands.ExecutedCommandActions.Interfaces;
using GothicModComposer.Core.Models.Profiles;
using GothicModComposer.Core.Models.VdfFiles;
using GothicModComposer.Core.Utils.IOHelpers;

namespace GothicModComposer.Core.Commands
{
    public class EnableVdfFilesCommand : ICommand
    {
        private static readonly Stack<ICommandActionVDF> ExecutedActions = new();

        private readonly IProfile _profile;

        public EnableVdfFilesCommand(IProfile profile)
            => _profile = profile;

        public string CommandName => "Enable VDF files";

        public async Task ExecuteAsync()
        {
            DirectoryHelper.GetAllFilesInDirectory(_profile.GothicFolder.DataFolderPath, SearchOption.TopDirectoryOnly)
                .ConvertAll(file => new VdfFile(file))
                .FindAll(vdf => vdf.IsDisabled && vdf.IsBaseVdf)
                .ForEach(vdf =>
                {
                    vdf.Enable();

                    ExecutedActions.Push(CommandActionVDF.FileEnabled(vdf));
                });
        }

        public void Undo() => ExecutedActions.Undo();
    }
}