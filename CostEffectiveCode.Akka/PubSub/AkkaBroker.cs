using System;
using Akka.Actor;
using CostEffectiveCode.Akka.Messages;
using CostEffectiveCode.Messaging;
using CostEffectiveCode.Domain.Cqrs.Commands;
using JetBrains.Annotations;
// ReSharper disable UseNameofExpression

namespace CostEffectiveCode.Akka.PubSub
{
    public class AkkaBroker<T> : IBroker<T>
    {
        // used directly with ICanTell (ActorRef or ActorSelection)
        private ICanTell _pubSubCoordinator;

        // or by conventions -- with actor path provided
        private readonly ActorSystem _system;
        private readonly string _actorPath;

        /// <summary>
        /// CEC Pub/sub broker implementation working on top of Akka.net actor system
        /// </summary>
        /// <param name="pubSubCoordinator">A reference to PubSubCoordinator&lt;T&gt; actor in your system (IActorRef or ActorSelection)</param>
        public AkkaBroker([NotNull] ICanTell pubSubCoordinator)
        {
            if (pubSubCoordinator == null) throw new ArgumentNullException("pubSubCoordinator");

            // TODO: implement an arbitrage and error checking -- need to ensure that provided pubSubCoordinator is really handled by PubSubCoordinatorActor class
            _pubSubCoordinator = pubSubCoordinator;
        }

        public AkkaBroker([NotNull] ActorSystem system, [NotNull] string actorPath, bool deferred = false)
        {
            if (system == null) throw new ArgumentNullException("system");
            if (actorPath == null) throw new ArgumentNullException("actorPath");

            if (!deferred)
            {
                _pubSubCoordinator = LoadPubSubCoordinator(system, actorPath);
            }
            else
            {
                _system = system;
                _actorPath = actorPath;
            }
        }

        private ICanTell LoadPubSubCoordinator(ActorSystem system, string actorPath)
        {
            var coordinatorSelection = system.ActorSelection(actorPath);

            if (coordinatorSelection == null)
                throw new ArgumentException("Wrong actor path provided: " + actorPath, "actorPath");

            return coordinatorSelection;
        }

        public void Publish(T message)
        {
            if (_pubSubCoordinator == null)
                _pubSubCoordinator = LoadPubSubCoordinator(_system, _actorPath);

            _pubSubCoordinator.Tell(message, null);
        }

        public void Subscribe(ICommand<T> handler)
        {
            if (_pubSubCoordinator == null)
                _pubSubCoordinator = LoadPubSubCoordinator(_system, _actorPath);

            _pubSubCoordinator.Tell(new SubscribeMessage<T>(handler), null);
        }

        public void Unsubscribe(ICommand<T> handler)
        {
            if (_pubSubCoordinator == null)
                _pubSubCoordinator = LoadPubSubCoordinator(_system, _actorPath);

            _pubSubCoordinator.Tell(new UnsubscribeMessage<T>(handler), null);
        }

        public bool IsSelfControlled { get { return true; } }
    }
}
