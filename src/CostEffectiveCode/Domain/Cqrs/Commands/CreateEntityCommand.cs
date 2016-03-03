using CostEffectiveCode.Common;
using CostEffectiveCode.Common.Scope;
using CostEffectiveCode.Domain.Ddd.Entities;
using CostEffectiveCode.Domain.Ddd.UnitOfWork;
using JetBrains.Annotations;

namespace CostEffectiveCode.Domain.Cqrs.Commands
{
    public class CreateEntityCommand<T> : UnitOfWorkScopeCommand<T>
        where T : class, IEntity
    {
        public override void Execute(T context)
        {
            // we don't check for Id, because sometimes we have to create entities with predefined Id value

            UnitOfWorkScope.Instance.Add(context);
            UnitOfWorkScope.Instance.Commit();
        }

        public CreateEntityCommand([NotNull] IScope<IUnitOfWork> unitOfWorkScope)
            : base(unitOfWorkScope)
        {
        }
    }
}
