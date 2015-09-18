using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CostEffectiveCode.Common.Messaging.Process
{
    [PublicAPI]
    public class ProcessFinishedEventArgs<TProcessResult> : ProcessEventArgsBase
    {
        public TProcessResult ProcessResult { get; set; }

        public ProcessFinishedEventArgs([NotNull] TProcessResult processResult, Guid processGuid) : base(processGuid)
        {
            if (processResult == null) throw new ArgumentNullException("processResult");

            ProcessResult = processResult;
        }
    }
}
