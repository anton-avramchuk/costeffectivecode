using CostEffectiveCode.Domain.Cqrs.Commands;

namespace CostEffectiveCode.Akka.Messages
{
    public class SubscribeMessage<T> : MessageWithCommandBase<T>
    {
        public SubscribeMessage(ICommand<T> command)
            : base(command)
        {
        }
    }
}
