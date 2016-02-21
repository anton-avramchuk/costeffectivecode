using CostEffectiveCode.Domain.Cqrs.Commands;

namespace CostEffectiveCode.Akka.Messages
{
    public abstract class MessageWithCommandBase<T>
    {
        protected MessageWithCommandBase(ICommand<T> command)
        {
            Command = command;
        }

        public ICommand<T> Command { get; }
    }
}