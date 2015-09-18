using System;
using JetBrains.Annotations;

namespace CostEffectiveCode.Domain.Cqrs.Commands
{
    [PublicAPI]
    public interface IQueuedCommand
    {
        TimeSpan Timeout { get; }
    }
}
