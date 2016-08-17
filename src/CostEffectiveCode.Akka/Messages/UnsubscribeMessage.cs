using CosteffectiveCode.Domain.Cqrs.Commands;

namespace CostEffectiveCode.Akka.Messages
{
    public class UnsubscribeMessage<T> : MessageWithCommandBase<T>
    {
        public UnsubscribeMessage(ICommand<T> command)
            : base(command)
        {
        }
    }
}
