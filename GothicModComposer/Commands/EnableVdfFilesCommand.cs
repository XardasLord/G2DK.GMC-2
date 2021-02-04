using System.IO;
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
					Logger.Info($"Enabled VDF file \"{vdf.FileNameWithoutExtension}\".");
				});
		}

		public void Undo()
		{
			Logger.Warn("Undo not implemented yet.");
		}
	}
}