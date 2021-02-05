using System.IO;
using System.Text.RegularExpressions;
using GothicModComposer.Utils;
using GothicModComposer.Utils.IOHelpers;

namespace GothicModComposer.Models.VdfFiles
{
	public class VdfFile
	{
		public string FilePath { get; }
		public string FileNameWithoutExtension { get; }
		public bool IsValidVdfFile { get; }
		public bool IsDisabled { get; }
		public bool IsBaseVdf { get; }

		private readonly Regex _regex = new(VdfFileHelper.VdfFileNameRegex, RegexOptions.IgnoreCase);
		private readonly string _folderPath;

		public VdfFile(string filePath)
		{
			FilePath = filePath;
			_folderPath = Path.GetDirectoryName(FilePath);

			var regexResult = _regex.Match(Path.GetFileName(filePath));

			FileNameWithoutExtension = regexResult.Groups["FileName"].Value;
			IsValidVdfFile = regexResult.Success;
			IsDisabled = regexResult.Groups["Disabled"].Success;
			IsBaseVdf = VdfFileHelper.IsBaseVdf(FileNameWithoutExtension);
		}

		public void Enable()
		{
			var paths = new VdfHelperPath(_folderPath, FileNameWithoutExtension);
			FileHelper.Move(paths.DisabledPath, paths.EnabledPath);

			Logger.Info($"Enabled VDF file \"{FileNameWithoutExtension}\".");
		}

		public void Disable()
		{
			var paths = new VdfHelperPath(_folderPath, FileNameWithoutExtension);
			FileHelper.Move(paths.EnabledPath, paths.DisabledPath);

			Logger.Info($"Disabled VDF file \"{FileNameWithoutExtension}\".");
		}

		private readonly struct VdfHelperPath
		{
			public string EnabledPath { get; }
			public string DisabledPath { get; }

			public VdfHelperPath(string dataFolder, string vdfName)
			{
				EnabledPath = Path.Combine(dataFolder, $"{vdfName}.vdf");
				DisabledPath = EnabledPath + ".disabled"; ;
			}
		}
	}
}