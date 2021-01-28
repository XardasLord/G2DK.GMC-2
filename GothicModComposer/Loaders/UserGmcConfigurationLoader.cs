using System;
using System.IO;
using System.Text.Json;
using GothicModComposer.Utils.Configurations;
using GothicModComposer.Utils.Exceptions;

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
			try
			{
				var jsonConfigurationFile = File.ReadAllText(filepath);
				return JsonSerializer.Deserialize<UserGmcConfiguration>(jsonConfigurationFile, new JsonSerializerOptions { AllowTrailingCommas = true });
			}
			catch (Exception e)
			{
				throw new ConfigurationFileNotFoundException(filepath, e);
			}
		}
	}
}