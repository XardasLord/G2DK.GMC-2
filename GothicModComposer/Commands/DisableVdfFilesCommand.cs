using System.Collections.Generic;
using System.IO;
using GothicModComposer.Commands.ExecutedCommandActions;
using GothicModComposer.Commands.ExecutedCommandActions.Interfaces;
using GothicModComposer.Models.Profiles;
using GothicModComposer.Models.VdfFiles;
using GothicModComposer.Utils.IOHelpers;

namespace GothicModComposer.Commands
{
	public class DisableVdfFilesCommand : ICommand
	{
		public string CommandName => "Disable VDF files";

		private readonly IProfile _profile;
		private static readonly Stack<ICommandActionVDF> ExecutedActions = new();

		public DisableVdfFilesCommand(IProfile profile) 
			=> _profile = profile;

		public void Execute()
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