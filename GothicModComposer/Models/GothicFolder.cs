namespace GothicModComposer.Models
{
	public class GothicFolder
	{
		public string BasePath { get; }

		private GothicFolder(string gothicFolderPath) 
			=> BasePath = gothicFolderPath;

		public static GothicFolder CreateFromPath(string gothicFolderPath)
		{
			var instance = new GothicFolder(gothicFolderPath);

			instance.Verify();

			return instance;
		}

		private void Verify()
		{
			// TODO: Verify if the folder exists and is correct
		}
	}
}