using System.IO;

namespace GothicModComposer.Models
{
	public class GothicFolder
	{
		public string BasePath { get; }
		public string SystemFolderPath => Path.Combine(BasePath, "System");
		public string WorkFolderPath => Path.Combine(BasePath, "_Work");
		public string DataFolderPath => Path.Combine(BasePath, "Data");
		public string WorkDataFolderPath => Path.Combine(WorkFolderPath, "Data");
		public string GmcIniFilePath => Path.Combine(SystemFolderPath, "GMC.ini");
		public string GothicIniFilePath => Path.Combine(SystemFolderPath, "Gothic.ini");
		public string GothicExeFilePath => Path.Combine(SystemFolderPath, "Gothic2.exe");
		public string GothicSrcFilePath => Path.Combine(WorkDataFolderPath, "Scripts", "Content", "Gothic.src");
		public string CutsceneFolderPath => Path.Combine(WorkDataFolderPath, "Scripts", "Content", "Cutscene");
		public string GothicVdfsToolFilePath => Path.Combine(WorkFolderPath, "Tools", "VDFS", "GothicVDFS.exe");

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