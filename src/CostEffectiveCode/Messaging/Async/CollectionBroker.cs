using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using CostEffectiveCode.Cqrs.Commands;
using JetBrains.Annotations;

namespace CostEffectiveCode.Messaging.Async
{
    public class CollectionBroker<T> : IBroker<T>
    {
        private readonly IProducerConsumerCollection<T> _collection;
        private readonly Observable.IObservable<T> _observable;

        public bool IsSelfControlled { get; private set; }

        /// <summary>
        /// </summary>
        /// <param name="collection">A collection instance. The most common case is ConcurrentQuery object.</param>
        /// <param name="observable">An observable instance. Passing this parameter you could choose between event and command-collection based implementations.</param>
        public CollectionBroker([NotNull] IProducerConsumerCollection<T> collection, Observable.IObservable<T> observable)
        {
            _collection = collection;
            _observable = observable;

            IsSelfControlled = false;
        }

        /// <summary>
        /// Dummy implementation of self-control based on TPL
        /// </summary>
        /// <param name="refreshPeriodSeconds"></param>
        public void RunSelfControl(double refreshPeriodSeconds)
        {
            IsSelfControlled = true;
            var wtoken = new CancellationTokenSource();

            Task.Run(async () => // <- marked async
            {
                while (!wtoken.IsCancellationRequested)
                {
                    ProcessCollection();
                    await Task.Delay((int)(refreshPeriodSeconds * 1000.0), wtoken.Token); // <- await with cancellation
                }
                IsSelfControlled = false;
            }, wtoken.Token);
        }

        public void Publish(T message)
        {
            if (!_collection.TryAdd(message))
                throw new InvalidOperationException("Failed to push the message");
        }

        public void Subscribe(ICommand<T> handler)
        {
            _observable.AddHandler(handler);
        }

        public void Unsubscribe(ICommand<T> handler)
        {
            _observable.RemoveHandler(handler);
        }

        public void ProcessCollection()
        {
            T ev;

            while (_collection.TryTake(out ev))
            {
                _observable.HandleMessage(ev);
            }
        }
    }
}