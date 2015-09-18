using CostEffectiveCode.Common;
using CostEffectiveCode.Domain.Ddd.Entities;
using CostEffectiveCode.Domain.Ddd.UnitOfWork;
using JetBrains.Annotations;

namespace CostEffectiveCode.Domain.Cqrs.Commands
{
    public class DeleteEntityCommand<T> : UnitOfWorkScopeCommand<T>
        where T: class, IEntity
    {
        public DeleteEntityCommand([NotNull] IScope<IUnitOfWork> unitOfWorkScope)
            : base(unitOfWorkScope)
        {
        }

        public override void Execute(T context)
        {
            UnitOfWorkScope.GetScoped().Delete(context);
            UnitOfWorkScope.GetScoped().Commit();
        }
    }
}
