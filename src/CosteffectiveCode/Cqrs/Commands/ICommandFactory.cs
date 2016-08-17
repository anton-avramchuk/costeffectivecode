using CosteffectiveCode.Ddd.Entities;
using JetBrains.Annotations;

namespace CosteffectiveCode.Cqrs.Commands
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

        SaveCommand<TEntity, TKey> GetCreateCommand<TKey, TEntity>()
            where TKey: struct
            where TEntity : EntityBase<TKey>;

        DeleteCommand<T> GetDeleteCommand<T>()
            where T : class, IEntity;
    }
}