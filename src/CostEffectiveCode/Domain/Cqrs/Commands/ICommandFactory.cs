using CostEffectiveCode.Domain.Ddd.Entities;
using JetBrains.Annotations;

namespace CostEffectiveCode.Domain.Cqrs.Commands
{
    [PublicAPI]
    public interface ICommandFactory
    {
        TCommand GetCommand<TInput, TCommand>()
            where TCommand : ICommand<TInput>;

        TCommand GetCommand<TInput, TResult, TCommand>()
            where TCommand : ICommand<TInput, TResult>
            where TResult : struct;

        TCommand GetCommand<TCommand>()
            where TCommand : ICommand;

        SaveEntityCommand<TKey, TEntity> GetCreateCommand<TKey, TEntity>()
            where TKey: struct
            where TEntity : EntityBase<TKey>;

        DeleteEntityCommand<T> GetDeleteCommand<T>()
            where T : class, IEntity;
    }
}