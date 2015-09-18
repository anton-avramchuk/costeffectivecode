using System;
using CostEffectiveCode.Domain.Cqrs.Commands;
using JetBrains.Annotations;

namespace CostEffectiveCode.Common
{
    [PublicAPI]
    public interface ITrigger
    {
        Guid Id { get; }

        DateTime StartDateTime { get; }

        DateTime EndDateTime { get; }

        DateTime GetNextFireDateTime();

    }

    [PublicAPI]
    public interface IScheduller
    {
        IScheduledCommand Add(ICommand command, ITrigger trigger);
    }

    public interface IScheduledCommand : ICommand
    {
        ITrigger Trigger { get; }
    }
}
