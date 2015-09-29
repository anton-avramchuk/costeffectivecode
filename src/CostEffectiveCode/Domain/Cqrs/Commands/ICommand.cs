using JetBrains.Annotations;

namespace CostEffectiveCode.Domain.Cqrs.Commands
{
    [PublicAPI]
    public interface ICommand
    {
        void Execute();
    }

    [PublicAPI]
    public interface ICommand<in T>
    {
        void Execute(T context);
    }
}
