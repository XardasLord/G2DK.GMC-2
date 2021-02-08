using System.Linq;
using System.Text.RegularExpressions;
using GothicModComposer.Models.Profiles;
using GothicModComposer.Utils.IOHelpers;

namespace GothicModComposer.Commands
{
	public class CopyEssentialFilesFromBackupCommand : ICommand
	{
		public string CommandName => "Copy essential files from backup";

		private readonly IProfile _profile;
		private readonly Regex _essentialFileRegex;

		public CopyEssentialFilesFromBackupCommand(IProfile profile)
		{
			_profile = profile;
			_essentialFileRegex = new Regex(_profile.GmcFolder.EssentialFilesRegexPattern);
		}

		public void Execute()
		{
			var workDataBackupFiles = DirectoryHelper.GetAllFilesInDirectory(_profile.GmcFolder.BackupWorkDataFolderPath);
			var essentialFiles = workDataBackupFiles.FindAll(IsEssential).ToList();

			essentialFiles.ForEach(file =>
			{
				var relativePath = DirectoryHelper.ToRelativePath(file, _profile.GmcFolder.BackupWorkDataFolderPath);
				var path = DirectoryHelper.MergeRelativePath(_profile.GothicFolder.WorkDataFolderPath, relativePath);
				
				FileHelper.CopyWithOverwrite(file, path);

				// TODO: ICommandActionIO.Push();
			});
		}

		public void Undo()
		{
			throw new System.NotImplementedException();
		}

		private bool IsEssential(string filePath)
		{
			var relativeFilePath = DirectoryHelper.ToRelativePath(filePath, _profile.GmcFolder.BackupWorkDataFolderPath);
			return _essentialFileRegex.IsMatch(relativeFilePath);
		}
	}
}