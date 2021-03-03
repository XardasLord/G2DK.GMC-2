namespace GothicModComposer.Utils.Exceptions
{
    public class VdfsGothicConfigurationNotFoundException : ConfigurationExceptionBase
    {
        public override string Code => "vdfs_gothic_configuration_not_found";

        public VdfsGothicConfigurationNotFoundException()
            : base("VDFS Gothic config section in gmc-2.json file not found.")
        {
        }
    }
}