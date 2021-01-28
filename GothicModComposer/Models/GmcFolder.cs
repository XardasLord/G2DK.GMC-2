namespace GothicModComposer.Models
{
	public class GmcFolder
	{
		public string BasePath { get; }

		private GmcFolder(string gmcFolderPath)
			=> BasePath = gmcFolderPath;

		public static GmcFolder CreateFromPath(string gmcFolderPath)
		{
			var instance = new GmcFolder(gmcFolderPath);

			instance.Verify();

			return instance;
		}

		private void Verify()
		{
			// TODO: Verify if the folder exists and is correct
		}
	}
}