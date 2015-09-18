using System;
using JetBrains.Annotations;

namespace CostEffectiveCode.Common.Messaging.Process
{
    public abstract class ProcessEventArgsBase : EventArgs
    {
        public Guid ProcessGuid { get; set; }

        protected ProcessEventArgsBase(Guid processGuid)
        {
            ProcessGuid = processGuid;
        }
    }
}
