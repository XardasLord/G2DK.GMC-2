﻿namespace GothicModComposer.Utils.Exceptions
{
    public class GmcFileNotFoundException : GMCExceptionBase
    {
        public GmcFileNotFoundException(string filePath)
            : base($"Cannot find configuration file under the following path: {filePath}")
            => FilePath = filePath;

        public override string Code => "gmc_configuration_file_not_found";
        public string FilePath { get; }
    }
}