using CostEffectiveCode.Ddd.Entities;
using CostEffectiveCode.Ddd.Specifications.UnitOfWork;
using JetBrains.Annotations;

namespace CostEffectiveCode.Cqrs.Commands
{
    public class DeleteCommand<TEntity> : UowBased, ICommand<TEntity>
        where TEntity : class, IEntity
    {
        public DeleteCommand([NotNull] IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public void Execute(TEntity context)
        {
            UnitOfWork.Delete(context);
            UnitOfWork.Commit();
        }

    }
}
