using System;
using Akka.Actor;
using CostEffectiveCode.Akka.Messages;
using CostEffectiveCode.Common.Logger;
using CostEffectiveCode.Common.Scope;
using CostEffectiveCode.Domain.Cqrs.Commands;
using JetBrains.Annotations;

namespace CostEffectiveCode.Akka.Actors
{
    public class CommandActor : ReceiveActor
    {
        private readonly IScope<ICommand> _commandScope;
        private readonly ILogger _logger;

        public CommandActor([NotNull] IScope<ICommand> commandScope, [CanBeNull] ILogger logger)
        {
            if (commandScope == null) throw new ArgumentNullException(nameof(commandScope));

            _commandScope = commandScope;
            _logger = logger;

            Receive<ExecuteCommandMessage>(x => Handle());
        }

        private void Handle()
        {
            _logger?.Debug($"CommandActor received message of type {typeof(ExecuteCommandMessage)}");

            var instance = _commandScope.Instance;

            if (instance == null)
                throw new InvalidOperationException("CommandActor execute operation failed: Command cannot be null");

            instance.Execute();
        }
    }


    public class CommandActor<TMessage> : ReceiveActor
    {
        private readonly IScope<ICommand<TMessage>> _commandScope;
        private readonly ILogger _logger;

        public CommandActor([NotNull] IScope<ICommand<TMessage>> commandScope, [CanBeNull] ILogger logger)
        {
            if (commandScope == null) throw new ArgumentNullException(nameof(commandScope));

            _commandScope = commandScope;
            _logger = logger;

            Receive<TMessage>(x => Handle(x));

            // in order to receive null-messages
            Receive<ExecuteCommandMessage<TMessage>>(x => Handle(x));
        }

        public CommandActor([NotNull] ICommand<TMessage> command, [CanBeNull] ILogger logger)
            : this(new PassThroughScope<ICommand<TMessage>>(command), logger)
        {
        }

        private void Handle(ExecuteCommandMessage<TMessage> message)
        {
            _logger?.Debug($"CommandActor received message of type {typeof(ExecuteCommandMessage<TMessage>)}");
            Handle(message.Message);
        }

        private void Handle(TMessage message)
        {
            _logger?.Debug($"CommandActor received message of type {message.GetType()} with value of {message}");

            var command = _commandScope.Instance;

            if (command == null)
                throw new InvalidOperationException("CommandActor execute operation failed: Command cannot be null");

            command.Execute(message);
        }
    }

}
