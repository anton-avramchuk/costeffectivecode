using System;
using JetBrains.Annotations;

namespace CostEffectiveCode.Domain.Cqrs.Commands
{
    public class ActionCommand<T> : ICommand<T>
    {
        private readonly Action<T> _action;

        public ActionCommand([NotNull] Action<T> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            _action = action;
        }

        public void Execute(T context)
        {
            _action.Invoke(context);
        }
    }
}
