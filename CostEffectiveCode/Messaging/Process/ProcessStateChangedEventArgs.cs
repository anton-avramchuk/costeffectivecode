using System;
using JetBrains.Annotations;

namespace CostEffectiveCode.Messaging.Process
{
    public class ProcessStateChangedEventArgs<TProcessState> : ProcessEventArgsBase
        where TProcessState: ProcessState
    {
        public ProcessStateChangedEventArgs([NotNull] TProcessState processState, Guid processGuid) : base(processGuid)
        {
            if (processState == null) throw new ArgumentNullException("processState");
            State = processState;
        }

        public TProcessState State { get; private set; }
    }
}
