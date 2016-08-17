using System;
using System.Collections.Generic;
using Akka.Actor;
using CosteffectiveCode.Domain.Cqrs.Commands;
using CostEffectiveCode.Akka.Messages;


namespace CostEffectiveCode.Akka.Actors
{
    /// <summary>
    /// Actor to be ruled by AkkaBroker&lt;T&gt;
    /// </summary>
    /// <typeparam name="T">Use the same T in AkkaBroker</typeparam>
    public class PubSubCoordinatorActor<T> : ReceiveActor
    {
        private readonly IDictionary<ICommand<T>, IActorRef> _subscribers;

        public PubSubCoordinatorActor()
        {
            Receive<T>(message => Publish(message));
            Receive<SubscribeMessage<T>>(message => Subscribe(message));
            Receive<UnsubscribeMessage<T>>(message => Unsubscribe(message));

            _subscribers = new Dictionary<ICommand<T>, IActorRef>();
        }

        private void Publish(T message)
        {
            var children = Context.GetChildren();

            if (children == null)
                throw new InvalidOperationException("Cannot publish " + typeof(T).Name + " message, 'cause no subscribers are available as a child actors of this Pub/Sub coordinator actor");

            foreach (var child in children)
                child.Tell(message);
        }

        private void Subscribe(SubscribeMessage<T> message)
        {
            // subscription means creation of child actor in terms of Akka
            var command = message.Command;
            _subscribers[command] = Context.ActorOf(Props.Create<CommandActor<T>>(command));
        }

        private void Unsubscribe(UnsubscribeMessage<T> message)
        {
            // unsubscription means killing the child actor
            // TODO: review whether to use PoisonPill, Kill, Stop or GracefulStop here
            _subscribers[message.Command].Tell(PoisonPill.Instance);
            _subscribers.Remove(message.Command);
        }
    }
}
