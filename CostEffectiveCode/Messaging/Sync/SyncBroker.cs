using System.Threading.Tasks;
using CostEffectiveCode.Domain.Cqrs.Commands;
using CostEffectiveCode.Messaging.Observable;

// ReSharper disable ConvertPropertyToExpressionBody

namespace CostEffectiveCode.Messaging.Sync
{
    public class SyncBroker<T> : IBroker<T>
    {
        private readonly IObservable<T> _observable;
        private readonly bool _nonBlocking;
        private T _recentMessage;

        public SyncBroker(IObservable<T> observable, bool nonBlocking = false)
        {
            _observable = observable;
            _nonBlocking = nonBlocking;
            _recentMessage = default(T);
        }

        public void Publish(T message)
        {
            _recentMessage = message;

            if (_nonBlocking)
            {
                Task.Run(() => _observable.HandleMessage(message));
                return;
            }

            _observable.HandleMessage(message);
        }

        public void Subscribe(ICommand<T> handler)
        {
            _observable.AddHandler(handler);
        }

        public void Unsubscribe(ICommand<T> handler)
        {
            _observable.RemoveHandler(handler);
        }

        public bool IsSelfControlled { get { return true; } }

        public T RecentMessage
        {
            get { return _recentMessage; }
        }
    }
}