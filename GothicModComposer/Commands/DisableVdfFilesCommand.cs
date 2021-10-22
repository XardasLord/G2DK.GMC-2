using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using GothicModComposer.Commands.ExecutedCommandActions;
using GothicModComposer.Commands.ExecutedCommandActions.Interfaces;
using GothicModComposer.Models.Profiles;
using GothicModComposer.Models.VdfFiles;
using GothicModComposer.Utils.IOHelpers;

namespace GothicModComposer.Commands
{
    public class DisableVdfFilesCommand : ICommand
    {
        private static readonly ConcurrentStack<ICommandActionVDF> ExecutedActions = new();

        private readonly IProfile _profile;

        public DisableVdfFilesCommand(IProfile profile)
            => _profile = profile;

        public string CommandName => "Disable VDF files";

        public void Execute()
        {
            var files = DirectoryHelper.GetAllFilesInDirectory(_profile.GothicFolder.DataFolderPath, SearchOption.TopDirectoryOnly)
                .ConvertAll(file => new VdfFile(file))
                .FindAll(vdf => vdf.IsEnabled && vdf.IsBaseVdf);

            Parallel.ForEach(files, vdf =>
            {
                vdf.Disable();
                ExecutedActions.Push(CommandActionVDF.FileDisabled(vdf));
            });
        }

        public void Undo() => ExecutedActions.Undo();
    }
}