using System;
using CostEffectiveCode.Cqrs.Commands;
using JetBrains.Annotations;

namespace CostEffectiveCode.Messaging.Observable
{
    /// <summary>
    /// Event-based implementation of IObservable&lt;T&gt;
    /// </summary>
    /// <typeparam name="T">type of message</typeparam>
    [PublicAPI]
    public class EventObservable<T> : IObservable<T>
    {
        private event Action<T> Event;

        // TODO: Need to check if Garbage Collector won't dispose handlers passed there.
        // In other case we need to store handlers as instances
        public void AddHandler(ICommandHandler<T> handler)
        {
            Event += handler.Handle;
        }

        public void AddHandler(Action<T> handler)
        {
            Event += handler;
        }

        public void RemoveHandler(ICommandHandler<T> handler)
        {
            Event -= handler.Handle;
        }

        public void RemoveHandler(Action<T> handler)
        {
            Event -= handler;
        }

        public void HandleMessage(T message)
        {
            Event?.Invoke(message);
        }
    }
}