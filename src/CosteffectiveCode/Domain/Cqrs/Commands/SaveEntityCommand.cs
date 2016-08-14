using System;
using CosteffectiveCode.Common;
using CosteffectiveCode.Domain.Ddd.Entities;
using CosteffectiveCode.Domain.Ddd.UnitOfWork;
using JetBrains.Annotations;

namespace CosteffectiveCode.Domain.Cqrs.Commands
{
    public class SaveEntityCommand<TKey,TEntity> : CommandBase<TEntity, TKey>
        where TKey: struct
        where TEntity : EntityBase<TKey>
    {
        private readonly IScope<IUnitOfWork> _unitOfWorkScope;

        public SaveEntityCommand([NotNull] IScope<IUnitOfWork> unitOfWorkScope)
        {
            if (unitOfWorkScope == null) throw new ArgumentNullException(nameof(unitOfWorkScope));
            _unitOfWorkScope = unitOfWorkScope;
        }

        protected override TKey DoExecute(TEntity context)
        {
            _unitOfWorkScope.Instance.Add(context);
            _unitOfWorkScope.Instance.Commit();
            return context.Id;
        }
    }
}
