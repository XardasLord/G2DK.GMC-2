using System;

namespace GothicModComposer.Utils.Exceptions
{
	public abstract class ConfigurationExceptionBase : Exception
	{
		public abstract string Code { get; }

		protected ConfigurationExceptionBase(string message) : base(message) { }
	}
}