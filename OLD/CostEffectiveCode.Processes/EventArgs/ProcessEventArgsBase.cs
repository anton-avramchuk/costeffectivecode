using System;
using JetBrains.Annotations;

namespace CostEffectiveCode.Processes.EventArgs
{
    [PublicAPI]
    public abstract class ProcessEventArgsBase : System.EventArgs
    {
        public Guid ProcessGuid { get; private set; }

        protected ProcessEventArgsBase(Guid processGuid)
        {
            ProcessGuid = processGuid;
        }
    }
}
