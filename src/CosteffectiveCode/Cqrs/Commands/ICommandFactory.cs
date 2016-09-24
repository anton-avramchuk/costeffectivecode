using CostEffectiveCode.Ddd.Entities;
using JetBrains.Annotations;

namespace CostEffectiveCode.Cqrs.Commands
{
    [PublicAPI]
    public interface ICommandFactory
    {
        TCommand GetCommand<TInput, TCommand>()
            where TCommand : ICommandHandler<TInput>;

        TCommand GetCommand<TInput, TResult, TCommand>()
            where TCommand : ICommandHandler<TInput, TResult>
            where TResult : struct;
        
        CreateCommandHandler<TKey, TDto, TEntity> GetCreateCommand<TKey, TDto, TEntity>()
            where TKey: struct
            where TEntity : HasIdBase<TKey>;

        DeleteCommandHandler<T> GetDeleteCommand<T>()
            where T : class, IHasId;
    }
}