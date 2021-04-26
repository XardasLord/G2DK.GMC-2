using CommandLine;
using GothicModComposer.Presets;

namespace GothicModComposer.Models
{
	public class GmcInitialParameter
	{
		[Option(
			'm',
			"modPath",
			Required = true,
			HelpText = "Absolute path to the History of Khorinis modification directory project.")]
        public string AbsolutePathToProject { get; set; }

        [Option(
            'g',
            "gothic2Path",
            Required = true,
            HelpText = "Absolute path to the Gothic II root directory game.")]
        public string AbsolutePathToGothic2Game { get; set; }

		[Option(
			'p',
			"profile",
			Required = true,
			HelpText = "Profile to determine how build should be executed.")]
		public ProfilePresetType Profile { get; set; }
		
		[Option(
			'c',
			"configurationFile",
			Required = false,
			HelpText = "Json configuration file for GMC.")]
		public string ConfigurationFile { get; set; }
	}
}