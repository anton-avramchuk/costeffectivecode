using JetBrains.Annotations;

namespace CosteffectiveCode.Cqrs.Commands
{
    [PublicAPI]
    public interface ICommand
    {
        void Execute();
    }

    [PublicAPI]
    public interface ICommand<in TInput>
    {
         void Execute(TInput input);
    }

    [PublicAPI]
    public interface ICommand<in TInput, out TResult>
        where TResult : struct
    {
         TResult Execute(TInput input);
    }
}
