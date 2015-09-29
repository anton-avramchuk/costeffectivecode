using System;
using CostEffectiveCode.Domain.Cqrs.Commands;
using JetBrains.Annotations;

namespace CostEffectiveCode.Messaging
{
    [PublicAPI]
    public interface ISubscriber
    {
        void Subscribe(ICommand handler);

        void Unsubscribe(ICommand handler);
    }

    [PublicAPI]
    public interface ISubscriber<out T>
    {
        void Subscribe(ICommand<T> handler);

        void Unsubscribe(ICommand<T> handler);
    }

    [PublicAPI]
    public static class SubscriberExtensions
    {
        public static ICommand<T> Subscribe<T>(this ISubscriber<T> subscriber, [NotNull] Action<T> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            var cmd = new ActionCommand<T>(action);
            subscriber.Subscribe(cmd);
            return cmd;
        }
    }
}
