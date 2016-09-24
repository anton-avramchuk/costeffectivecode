using CostEffectiveCode.Cqrs.Commands;
using JetBrains.Annotations;

namespace CostEffectiveCode.Messaging
{
    [PublicAPI]
    public interface ISubscriber<out T>
    {
        void Subscribe(ICommandHandler<T> handler);

        void Unsubscribe(ICommandHandler<T> handler);
    }
}
