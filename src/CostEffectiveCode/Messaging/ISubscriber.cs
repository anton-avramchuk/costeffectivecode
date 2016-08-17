using System;
using CosteffectiveCode.Cqrs.Commands;
using JetBrains.Annotations;

namespace CosteffectiveCode.Messaging
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
}
