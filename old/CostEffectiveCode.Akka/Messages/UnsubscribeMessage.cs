using CostEffectiveCode.Domain.Cqrs.Commands;

namespace CostEffectiveCode.Akka.Messages
{
    public class UnsubscribeMessage<T> : HasCommandMessageBase<T>
    {
        public UnsubscribeMessage(ICommand<T> command)
            : base(command)
        {
        }
    }
}
