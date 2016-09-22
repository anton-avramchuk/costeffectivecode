using CostEffectiveCode.Ddd.Entities;
using JetBrains.Annotations;

namespace CostEffectiveCode.Cqrs.Commands
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

        CreateCommand<TKey, TDto, TEntity> GetCreateCommand<TKey, TDto, TEntity>()
            where TKey: struct
            where TEntity : Entity<TKey>;

        DeleteCommand<T> GetDeleteCommand<T>()
            where T : class, IEntity;
    }
}