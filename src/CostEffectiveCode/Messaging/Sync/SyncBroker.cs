using System.Threading.Tasks;
using CostEffectiveCode.Cqrs.Commands;
using CostEffectiveCode.Messaging.Observable;
using JetBrains.Annotations;

namespace CostEffectiveCode.Messaging.Sync
{
    [PublicAPI]
    public class SyncBroker<T> : IBroker<T>
    {
        private readonly IObservable<T> _observable;
        private readonly bool _nonBlocking;

        public SyncBroker(IObservable<T> observable, bool nonBlocking = false)
        {
            _observable = observable;
            _nonBlocking = nonBlocking;
            RecentMessage = default(T);
        }

        public void Publish(T message)
        {
            RecentMessage = message;

            if (_nonBlocking)
            {
                Task.Run(() => _observable.HandleMessage(message));
                return;
            }

            _observable.HandleMessage(message);
        }

        public void Subscribe(ICommandHandler<T> handler)
        {
            _observable.AddHandler(handler);
        }

        public void Unsubscribe(ICommandHandler<T> handler)
        {
            _observable.RemoveHandler(handler);
        }

        public T RecentMessage { get; private set; }
    }
}