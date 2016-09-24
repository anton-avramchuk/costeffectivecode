using System.Collections.Generic;
using System.Linq;
using CostEffectiveCode.Cqrs.Commands;
using JetBrains.Annotations;

namespace CostEffectiveCode.Messaging.Observable
{
    /// <summary>
    /// Based on the collection of ICommand&lt;T&gt; implementation of IObservable&lt;T&gt;
    /// </summary>
    /// <typeparam name="T">type of message</typeparam>
    [PublicAPI]
    public class CommandsCollectionObservable<T> : IObservable<T>
    {
        private readonly ICollection<ICommandHandler<T>> _handlersCollection;

        public CommandsCollectionObservable()
        {
            _handlersCollection = new List<ICommandHandler<T>>();
        }

        public CommandsCollectionObservable(IEnumerable<ICommandHandler<T>> handlersEnumerable)
        {
            _handlersCollection = new List<ICommandHandler<T>>(handlersEnumerable);
        }

        public CommandsCollectionObservable(params ICommandHandler<T>[] handlers)
        {
            _handlersCollection = new List<ICommandHandler<T>>(handlers);
        }

        public void AddHandler(ICommandHandler<T> handler)
        {
            _handlersCollection.Add(handler);
        }

        public void RemoveHandler(ICommandHandler<T> handler)
        {
            _handlersCollection.Remove(handler);
        }

        public void HandleMessage(T message)
        {
            var list = _handlersCollection as List<ICommandHandler<T>> ?? _handlersCollection.ToList();

            list.ForEach(x => x.Handle(message));
        }
    }
}
