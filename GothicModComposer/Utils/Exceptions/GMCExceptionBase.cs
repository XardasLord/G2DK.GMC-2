using System;

namespace GothicModComposer.Utils.Exceptions
{
	public abstract class GMCExceptionBase : Exception
	{
		public abstract string Code { get; }

		protected GMCExceptionBase(string message) : base(message) { }
	}
}