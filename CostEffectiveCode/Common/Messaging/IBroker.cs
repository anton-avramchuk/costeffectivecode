using JetBrains.Annotations;

namespace CostEffectiveCode.Common.Messaging
{
    [PublicAPI]
    public interface IBroker : IPublisher, ISubscriber
    {
        bool IsSelfControlled { get; }
    }

    [PublicAPI]
    public interface IBroker<T> : IPublisher<T>, ISubscriber<T>
    {
        bool IsSelfControlled { get; }
    }
}
