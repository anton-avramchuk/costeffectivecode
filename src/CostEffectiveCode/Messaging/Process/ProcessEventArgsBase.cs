using System;
using JetBrains.Annotations;

namespace CostEffectiveCode.Messaging.Process
{
    [PublicAPI]
    public abstract class ProcessEventArgsBase : EventArgs
    {
        public Guid ProcessGuid { get; private set; }

        protected ProcessEventArgsBase(Guid processGuid)
        {
            ProcessGuid = processGuid;
        }
    }
}
