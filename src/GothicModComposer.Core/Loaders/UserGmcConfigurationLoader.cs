using System.IO;
using System.Text.Json;
using GothicModComposer.Core.Models.Configurations;
using GothicModComposer.Core.Utils.Exceptions;
using GothicModComposer.Core.Utils.IOHelpers;

namespace GothicModComposer.Core.Loaders
{
    public static class UserGmcConfigurationLoader
    {
        private const string GmcConfigurationFileName = "gmc-2.json";

        public static UserGmcConfiguration Load(string projectRootDirectory, string configurationFile)
        {
            var gmcConfigurationFilePath =
                configurationFile ?? Path.Combine(projectRootDirectory, GmcConfigurationFileName);

            var userGmcConfig = DeserializeConfigurationFromFile(gmcConfigurationFilePath);

            return userGmcConfig;
        }

        private static UserGmcConfiguration DeserializeConfigurationFromFile(string filepath)
        {
            if (!FileHelper.Exists(filepath))
                throw new GmcFileNotFoundException(filepath);

            var jsonConfigurationFile = FileHelper.ReadFile(filepath);
            return JsonSerializer.Deserialize<UserGmcConfiguration>(jsonConfigurationFile,
                new JsonSerializerOptions {AllowTrailingCommas = true});
        }
    }
}