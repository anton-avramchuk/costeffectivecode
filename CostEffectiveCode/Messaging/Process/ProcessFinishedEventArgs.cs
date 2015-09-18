using System;
using JetBrains.Annotations;

namespace CostEffectiveCode.Messaging.Process
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
