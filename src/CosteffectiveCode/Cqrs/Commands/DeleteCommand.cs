using CostEffectiveCode.Ddd.Entities;
using CostEffectiveCode.Ddd.Specifications.UnitOfWork;
using JetBrains.Annotations;

namespace CostEffectiveCode.Cqrs.Commands
{
    public class DeleteCommandHandler<TEntity> : UowBased, ICommandHandler<TEntity>
        where TEntity : class, IHasId
    {
        public DeleteCommandHandler([NotNull] IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public void Handle(TEntity context)
        {
            UnitOfWork.Delete(context);
            UnitOfWork.Commit();
        }

    }
}
