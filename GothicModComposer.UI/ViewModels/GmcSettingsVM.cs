using System;
using System.IO;
using System.Text.Json;
using GothicModComposer.UI.Helpers;
using GothicModComposer.UI.Models;

namespace GothicModComposer.UI.ViewModels
{
    public class GmcSettingsVM : ObservableVM
    {
        public string GmcSettingsJsonFilePath { get; }

        public GmcSettingsVM()
        {
            GmcSettingsJsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "gmc-2-ui.json");

            if (!File.Exists(GmcSettingsJsonFilePath))
                CreateDefaultConfigurationFile();
        }

        private void CreateDefaultConfigurationFile()
        {
            var configurationFile = GmcConfiguration.CreateDefault();

            var configurationJson = JsonSerializer.Serialize(configurationFile);

            File.WriteAllText(GmcSettingsJsonFilePath, configurationJson);
        }
    }
}