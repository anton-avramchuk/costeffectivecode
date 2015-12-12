using System;
using JetBrains.Annotations;

namespace CostEffectiveCode.Processes.EventArgs
{
    [PublicAPI]
    public class StopProcessEventArgs : ProcessEventArgsBase
    {
        public StopProcessEventArgs(Guid processGuid) : base(processGuid)
        {
        }
    }
}
