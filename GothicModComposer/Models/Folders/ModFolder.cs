using System.IO;

namespace GothicModComposer.Models.Folders
{
	public class ModFolder
	{
		public string BasePath { get; }
		public string ExtensionsFolderPath => Path.Combine(BasePath, "Extensions");

		private ModFolder(string modFolderPath)
			=> BasePath = modFolderPath;

		public static ModFolder CreateFromPath(string modFolderPath)
		{
			var instance = new ModFolder(modFolderPath);

			instance.Verify();

			return instance;
		}

		private void Verify()
		{
			// TODO: Verify if the folder exists and is correct
		}
	}
}