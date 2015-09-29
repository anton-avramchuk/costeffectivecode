using System;
using Akka.Actor;
using CostEffectiveCode.Domain.Cqrs.Commands;
using JetBrains.Annotations;

namespace CostEffectiveCode.Akka.Actors
{

    public class CommandActor<T> : ReceiveActor
    {
        private readonly ICommand<T> _command;

        public CommandActor([NotNull] ICommand<T> command)
        {
            if (command == null) throw new ArgumentNullException("command");
            _command = command;

            Receive<T>(message => _command.Execute(message));
        }
    }
}
