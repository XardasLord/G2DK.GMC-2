using System;

namespace GothicModComposer.Utils.Exceptions
{
	public class ConfigurationFileNotFoundException : Exception
	{
		public string FilePath { get; }
		public Exception Exception { get; }

		public ConfigurationFileNotFoundException(string filePath, Exception ex) 
			: base($"Cannot find configuration file under the path: {filePath}", ex)
		{
			FilePath = filePath;
			Exception = ex;
		}
	}
}