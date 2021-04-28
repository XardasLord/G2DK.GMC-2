namespace GothicModComposer.Utils.Exceptions
{
    public class VdfsGothicGmcNotFoundException : GMCExceptionBase
    {
        public override string Code => "vdfs_gothic_configuration_not_found";

        public VdfsGothicGmcNotFoundException()
            : base("VDFS Gothic config section in json configuration file was not found.")
        {
        }
    }
}