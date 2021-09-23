namespace GothicModComposer.Utils.Exceptions
{
    public class DefaultWorldNotFoundException : GMCExceptionBase
    {
        public DefaultWorldNotFoundException()
            : base("DefaultWorld value has to be specify in the json configuration file for this profile.")
        {
        }

        public override string Code => "default_world_configuration_value_not_found";
    }
}