using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace CostEffectiveCode.Messaging.Process
{
    public abstract class ProcessExceptionBase : Exception
    {
        protected ProcessExceptionBase()
        {
        }

        protected ProcessExceptionBase(string message)
            : base(message)
        {
        }

        protected ProcessExceptionBase(string message, Exception innerException)
            : base(message, innerException)
        {
            InnerException = innerException;
        }

        protected ProcessExceptionBase([NotNull] SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public new Exception InnerException
        {
            get; set;
        }

    }
}
