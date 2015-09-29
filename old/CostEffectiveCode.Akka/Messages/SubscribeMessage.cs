using CostEffectiveCode.Domain.Cqrs.Commands;

namespace CostEffectiveCode.Akka.Messages
{
    public class SubscribeMessage<T> : HasCommandMessageBase<T>
    {
        public SubscribeMessage(ICommand<T> command)
            : base(command)
        {
        }
    }
}
