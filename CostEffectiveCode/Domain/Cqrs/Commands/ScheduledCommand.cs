using System;
using CostEffectiveCode.Common;
using JetBrains.Annotations;

namespace CostEffectiveCode.Domain.Cqrs.Commands
{
    public class ScheduledCommand : IScheduledCommand
    {
        protected readonly ICommand Command;
        
        public ScheduledCommand([NotNull] ICommand command, [NotNull] ITrigger trigger)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (trigger == null) throw new ArgumentNullException("trigger");
            Command = command;
            Trigger = trigger;
        }

        public void Execute()
        {
            Command.Execute();
        }

        public ITrigger Trigger { get; private set; }

    }
}
