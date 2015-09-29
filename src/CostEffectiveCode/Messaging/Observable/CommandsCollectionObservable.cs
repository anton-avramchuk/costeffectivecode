using System.Collections.Generic;
using System.Linq;
using CostEffectiveCode.Domain.Cqrs.Commands;
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
        private readonly ICollection<ICommand<T>> _handlersCollection;

        public CommandsCollectionObservable()
        {
            _handlersCollection = new List<ICommand<T>>();
        }

        public CommandsCollectionObservable(IEnumerable<ICommand<T>> handlersEnumerable)
        {
            _handlersCollection = new List<ICommand<T>>(handlersEnumerable);
        }

        public CommandsCollectionObservable(params ICommand<T>[] handlers)
        {
            _handlersCollection = new List<ICommand<T>>(handlers);
        }

        public void AddHandler(ICommand<T> handler)
        {
            _handlersCollection.Add(handler);
        }

        public void RemoveHandler(ICommand<T> handler)
        {
            _handlersCollection.Remove(handler);
        }

        public void HandleMessage(T message)
        {
            var list = _handlersCollection as List<ICommand<T>> ?? _handlersCollection.ToList();

            list.ForEach(x => x.Execute(message));
        }
    }
}
