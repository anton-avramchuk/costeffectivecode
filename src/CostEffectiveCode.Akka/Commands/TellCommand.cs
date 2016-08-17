using Akka.Actor;
using CosteffectiveCode.Domain.Cqrs.Commands;
using JetBrains.Annotations;

namespace CostEffectiveCode.Akka.Cqrs
{
    public class TellCommand<T> : ICommand<T>
    {
        private readonly ICanTell _dest;
        private readonly IActorRef _sender;

        /// <summary>
        /// Tell to Actor or ActorSelection command (in terms of CostEffectiveCode CQRS)
        /// </summary>
        /// <param name="dest">ICanTell object - normally Actor or ActorSelection</param>
        /// <param name="sender">Normally null, but could be specified explicitly</param>
        public TellCommand([NotNull] ICanTell dest, [CanBeNull] IActorRef sender = null)
        {
            _dest = dest;

            // Hint: nulled sender should be converted to NoSender object inside of ICanTell implementation (ActorRef or ActorSelection)
            _sender = sender;
        }

        public void Execute(T message)
        {
            _dest.Tell(message, _sender);
        }
    }
}
