using System.IO;
using System.Text.Json;
using GothicModComposer.Models.Configurations;
using GothicModComposer.Utils.Exceptions;
using GothicModComposer.Utils.IOHelpers;

namespace GothicModComposer.Loaders
{
	public static class UserGmcConfigurationLoader
	{
		private const string GmcConfigurationFileName = "gmc.json";

		public static UserGmcConfiguration Load(string projectRootDirectory)
		{
			var gmcConfigurationFilePath = Path.Combine(projectRootDirectory, GmcConfigurationFileName);

			var userGmcConfig = DeserializeConfigurationFromFile(gmcConfigurationFilePath);

			return userGmcConfig;
		}

		private static UserGmcConfiguration DeserializeConfigurationFromFile(string filepath)
		{
			if (!FileHelper.Exists(filepath))
				throw new ConfigurationFileNotFoundException(filepath);

			var jsonConfigurationFile = FileHelper.ReadFile(filepath);
			return JsonSerializer.Deserialize<UserGmcConfiguration>(jsonConfigurationFile, new JsonSerializerOptions { AllowTrailingCommas = true });
		}
	}
}