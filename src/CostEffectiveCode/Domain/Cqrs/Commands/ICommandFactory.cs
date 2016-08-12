using CostEffectiveCode.Domain.Ddd.Entities;
using JetBrains.Annotations;

namespace CostEffectiveCode.Domain.Cqrs.Commands
{
    [PublicAPI]
    public interface ICommandFactory
    {
        TCommand GetCommand<T, TCommand>()
            where TCommand : ICommand<T>;

        TCommand GetCommand<T, TResult, TCommand>()
            where TCommand : ICommand<T, TResult>
            where TResult : struct;

        T GetCommand<T>()
            where T : ICommand;

        CreateEntityCommand<TEntity> GetCreateCommand<TEntity>()
            where TEntity : class, IEntity;

        DeleteEntityCommand<T> GetDeleteCommand<T>()
            where T : class, IEntity;

        CommitCommand GetCommitCommand();
    }
}