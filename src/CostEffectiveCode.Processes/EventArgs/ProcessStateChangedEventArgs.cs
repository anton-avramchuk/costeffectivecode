using System;
using CostEffectiveCode.Processes.State;
using JetBrains.Annotations;

namespace CostEffectiveCode.Processes.EventArgs
{
    [PublicAPI]
    public class ProcessStateChangedEventArgs<TProcessState> : ProcessEventArgsBase
        where TProcessState: ProcessState
    {
        public ProcessStateChangedEventArgs([NotNull] TProcessState processState, Guid processGuid)
            : base(processGuid)
        {
            if (processState == null) throw new ArgumentNullException(nameof(processState));
            State = processState;
        }

        public TProcessState State { get; private set; }
    }
}
