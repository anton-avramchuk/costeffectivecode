using CostEffectiveCode.Domain.Ddd.Entities;
using JetBrains.Annotations;

namespace CostEffectiveCode.Domain.Cqrs.Commands
{
    [PublicAPI]
    public interface ICommandFactory
    {
        TCommand GetCommand<TEntity, TCommand>()
            where TCommand : ICommand<TEntity>
            where TEntity: IEntity;

        T GetCommand<T>()
            where T : ICommand;

        CreateEntityCommand<TEntity> GetCreateCommand<TEntity>()
            where TEntity : class, IEntity;

        DeleteEntityCommand<T> GetDeleteCommand<T>()
            where T : class, IEntity;

        CommitCommand GetCommitCommand();
    }
}