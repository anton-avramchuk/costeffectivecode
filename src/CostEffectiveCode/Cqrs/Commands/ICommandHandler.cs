using JetBrains.Annotations;

namespace CostEffectiveCode.Cqrs.Commands
{
    [PublicAPI]
    public interface ICommandHandler<in TInput>
    {
         void Handle(TInput input);
    }

    [PublicAPI]
    public interface ICommandHandler<in TInput, out TResult>
        where TResult : struct
    {
         TResult Handle(TInput input);
    }
}
