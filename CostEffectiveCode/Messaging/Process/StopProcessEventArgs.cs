using System;
using JetBrains.Annotations;

namespace CostEffectiveCode.Messaging.Process
{
    [PublicAPI]
    public class StopProcessEventArgs : ProcessEventArgsBase
    {
        public StopProcessEventArgs(Guid processGuid) : base(processGuid)
        {
        }
    }
}
