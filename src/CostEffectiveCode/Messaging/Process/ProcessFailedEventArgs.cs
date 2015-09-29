using System;
using JetBrains.Annotations;

namespace CostEffectiveCode.Messaging.Process
{
    [PublicAPI]
    public class ProcessFailedEventArgs<TProcessException> : ProcessEventArgsBase
        where TProcessException : ProcessExceptionBase, new()
    {
        public TProcessException Error { get; set; }

        public ProcessFailedEventArgs([CanBeNull] TProcessException error, Guid processGuid)
            : base(processGuid)
        {
            Error = error;
        }
    }
}
