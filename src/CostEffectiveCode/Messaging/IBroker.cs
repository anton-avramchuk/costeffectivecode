using JetBrains.Annotations;

namespace CostEffectiveCode.Messaging
{
    [PublicAPI]
    public interface IBroker<T> : IPublisher<T>, ISubscriber<T>
    {
    }
}
