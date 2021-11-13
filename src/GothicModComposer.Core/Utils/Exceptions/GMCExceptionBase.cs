using System;

namespace GothicModComposer.Core.Utils.Exceptions
{
    public abstract class GMCExceptionBase : Exception
    {
        protected GMCExceptionBase(string message) : base(message)
        {
        }

        public abstract string Code { get; }
    }
}