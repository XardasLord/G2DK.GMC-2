namespace GothicModComposer.Utils.Exceptions
{
	public class DefaultWorldNotFoundException : ConfigurationExceptionBase
	{
		public override string Code => "default_world_configuration_value_not_found";

		public DefaultWorldNotFoundException()
			: base("DefaultWorld value has to be specify in the gmc.json configuration file for this profile.")
		{
		}
	}
}