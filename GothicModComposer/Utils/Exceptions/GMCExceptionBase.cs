using System;

namespace GothicModComposer.Utils.Exceptions
{
    public abstract class GMCExceptionBase : Exception
    {
        protected GMCExceptionBase(string message) : base(message)
        {
        }

        public abstract string Code { get; }
    }
}