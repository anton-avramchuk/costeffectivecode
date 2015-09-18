using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CostEffectiveCode.Common.Messaging.Process
{
    [PublicAPI]
    public class ProcessFailedEventArgs<TProcessException> : ProcessEventArgsBase
        where TProcessException : ProcessExceptionBase, new()     {
        public TProcessException Error { get; set; }

        public ProcessFailedEventArgs([CanBeNull] TProcessException error, Guid processGuid) : base(processGuid)
        {
            Error = error;
        }
    }
}
