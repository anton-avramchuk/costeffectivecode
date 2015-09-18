using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CostEffectiveCode.Common.Messaging.Process
{
    [PublicAPI]
    public class StopProcessEventArgs : ProcessEventArgsBase
    {
        public StopProcessEventArgs(Guid processGuid) : base(processGuid)
        {
        }
    }
}
