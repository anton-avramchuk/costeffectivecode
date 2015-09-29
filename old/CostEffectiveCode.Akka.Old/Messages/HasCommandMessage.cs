using CostEffectiveCode.Domain.Cqrs.Commands;
// ReSharper disable ConvertToAutoProperty
// ReSharper disable ConvertPropertyToExpressionBody

namespace CostEffectiveCode.Akka.Messages
{
    public abstract class HasCommandMessageBase<T>
    {
        private readonly ICommand<T> _command;

        protected HasCommandMessageBase(ICommand<T> command)
        {
            _command = command;
        }

        public ICommand<T> Command
        {
            get { return _command; }
        }
    }
}