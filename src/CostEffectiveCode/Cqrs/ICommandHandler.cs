using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CostEffectiveCode.Cqrs
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

    [PublicAPI]
    public interface IAsyncCommandHandler<in TInput>
    {
        Task Handle(TInput input);
    }

    [PublicAPI]
    public interface IAsyncCommandHandler<in TInput, TResult>
        where TResult : struct
    {
         Task<TResult> Handle(TInput input);
    }
}
