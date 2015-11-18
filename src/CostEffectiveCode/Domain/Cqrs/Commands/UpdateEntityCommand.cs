using System;
using CostEffectiveCode.Common;
using CostEffectiveCode.Domain.Ddd.Entities;
using CostEffectiveCode.Domain.Ddd.UnitOfWork;
using JetBrains.Annotations;

namespace CostEffectiveCode.Domain.Cqrs.Commands
{
    public class UpdateEntityCommand<T> : UnitOfWorkScopeCommand<T>
        where T : class, IEntity
    {
        public override void Execute(T context)
        {
            // TODO: fix later
            //if (context.GetId() == null)
            //{
            //    throw new ArgumentException("Given entity has no Id specified - so it cannot be updated", nameof(context));
            //}

            UnitOfWorkScope.Instance.Save(context);
            UnitOfWorkScope.Instance.Commit();
        }

        public UpdateEntityCommand([NotNull] IScope<IUnitOfWork> unitOfWorkScope)
            : base(unitOfWorkScope)
        {
        }
    }
}
