using CostEffectiveCode.Domain.Cqrs.Commands;

namespace CostEffectiveCode.Akka.Messages
{
    public abstract class HasCommandMessageBase<T>
    {
        protected HasCommandMessageBase(ICommand<T> command)
        {
            Command = command;
        }

        public ICommand<T> Command { get; }
    }
}