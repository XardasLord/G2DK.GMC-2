using System.Collections.Generic;
using System.IO;
using System.Linq;
using GothicModComposer.Commands.ExecutedCommandActions;
using GothicModComposer.Commands.ExecutedCommandActions.Interfaces;
using GothicModComposer.Models.Profiles;
using GothicModComposer.Models.VdfFiles;
using GothicModComposer.Utils;
using GothicModComposer.Utils.IOHelpers;

namespace GothicModComposer.Commands
{
	public class EnableVdfFilesCommand : ICommand
	{
		public string CommandName => "Enable VDF files";

		private readonly IProfile _profile;
		private static readonly Stack<ICommandActionVDF> ExecutedActions = new();

		public EnableVdfFilesCommand(IProfile profile) 
			=> _profile = profile;

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

		public void Undo()
		{
			if (!ExecutedActions.Any())
			{
				Logger.Info("There is nothing to undo, because no actions were executed.");
				return;
			}

			while (ExecutedActions.Count > 0)
			{
				var executedAction = ExecutedActions.Pop();
				executedAction?.Undo();
			}
		}
	}
}