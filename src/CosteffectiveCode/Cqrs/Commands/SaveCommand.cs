using CosteffectiveCode.Ddd.Entities;
using CosteffectiveCode.Ddd.Specifications.UnitOfWork;
using JetBrains.Annotations;

namespace CosteffectiveCode.Cqrs.Commands
{
    public class SaveCommand<TEntity, TKey> : UowBased, ICommand<TEntity, TKey>
        where TKey: struct
        where TEntity : EntityBase<TKey>
    {
        public SaveCommand([NotNull] IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public TKey Execute(TEntity context)
        {
            UnitOfWork.Add(context);
            UnitOfWork.Commit();
            return context.Id;
        }

    }
}
