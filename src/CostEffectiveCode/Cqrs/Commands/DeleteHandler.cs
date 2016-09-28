using System;
using CostEffectiveCode.Ddd;
using CostEffectiveCode.Ddd.Entities;
using JetBrains.Annotations;

namespace CostEffectiveCode.Cqrs.Commands
{
    public class DeleteHandler<TKey, TEntity>
        : UowBased
        , ICommandHandler<TKey>
        where TEntity : class, IHasId<TKey>
    {
        public DeleteHandler([NotNull] IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public void Handle(TKey key)
        {
            var entity = UnitOfWork.Find<TEntity>(key);
            if (entity == null)
            {
                throw new ArgumentException($"Entity {typeof(TEntity).Name} with id={key} doesn't exists");
            }

            UnitOfWork.Delete(entity);
            UnitOfWork.Commit();
        }

    }
}
