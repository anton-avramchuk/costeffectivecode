using CostEffectiveCode.Domain.Cqrs.Commands;
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
        void AddHandler(ICommand<T> handler);

        // TODO: check the need in RemoveHandler methods
        void RemoveHandler(ICommand<T> handler);

        void HandleMessage(T message);
    }
}
