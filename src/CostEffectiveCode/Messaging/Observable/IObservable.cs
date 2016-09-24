using CostEffectiveCode.Cqrs.Commands;
using JetBrains.Annotations;

namespace CostEffectiveCode.Messaging.Observable
{
    /// <summary>
    /// This interface provides API over standard .NET events mechanism
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [PublicAPI]
    public interface IObservable<T>
    {
        void AddHandler(ICommandHandler<T> handler);

        void RemoveHandler(ICommandHandler<T> handler);

        void HandleMessage(T message);
    }
}
