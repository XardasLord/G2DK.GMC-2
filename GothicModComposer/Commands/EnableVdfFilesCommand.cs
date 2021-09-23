using System.Collections.Generic;
using System.IO;
using GothicModComposer.Commands.ExecutedCommandActions;
using GothicModComposer.Commands.ExecutedCommandActions.Interfaces;
using GothicModComposer.Models.Profiles;
using GothicModComposer.Models.VdfFiles;
using GothicModComposer.Utils.IOHelpers;

namespace GothicModComposer.Commands
{
    public class EnableVdfFilesCommand : ICommand
    {
        private static readonly Stack<ICommandActionVDF> ExecutedActions = new();

        private readonly IProfile _profile;

        public EnableVdfFilesCommand(IProfile profile)
            => _profile = profile;

        public string CommandName => "Enable VDF files";

        public void Execute()
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