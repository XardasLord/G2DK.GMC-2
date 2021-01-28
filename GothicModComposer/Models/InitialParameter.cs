using CommandLine;
using GothicModComposer.Presets;

namespace GothicModComposer.Models
{
	public class InitialParameter
	{
		[Option(
			'p',
			"path",
			Required = true,
			HelpText = "Absolute path to the History of Khorinis modification directory project.")]
		public string AbsolutePathToProject { get; set; }

		[Option(
			'P',
			"profile",
			Required = true,
			HelpText = "Profile to determine how build should be executed.")]
		public ProfilePresetType Profile { get; set; }
	}
}