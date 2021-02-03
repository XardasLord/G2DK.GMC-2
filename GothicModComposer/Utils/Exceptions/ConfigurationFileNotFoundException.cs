namespace GothicModComposer.Utils.Exceptions
{
	public class ConfigurationFileNotFoundException : ConfigurationExceptionBase
	{
		public override string Code => "gmc_configuration_file_not_found";
		public string FilePath { get; }

		public ConfigurationFileNotFoundException(string filePath) 
			: base($"Cannot find configuration file under the path: {filePath}")
		{
			FilePath = filePath;
		}
	}
}