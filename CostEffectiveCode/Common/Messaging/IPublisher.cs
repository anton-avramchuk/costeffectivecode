using JetBrains.Annotations;

namespace CostEffectiveCode.Common.Messaging
{
    [PublicAPI]
    public interface IPublisher
    {
        void Publish(object message);
    }

    [PublicAPI]
    public interface IPublisher<in T>
    {
        void Publish(T message);
    }
}
