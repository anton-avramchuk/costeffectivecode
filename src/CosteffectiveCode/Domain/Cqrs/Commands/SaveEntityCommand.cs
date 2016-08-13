using System;
using CosteffectiveCode.Common;
using CostEffectiveCode.Domain.Ddd.Entities;
using CostEffectiveCode.Domain.Ddd.UnitOfWork;
using JetBrains.Annotations;

namespace CostEffectiveCode.Domain.Cqrs.Commands
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

        public override string Name => "Create";

        public override string Description => "Save entity";
    }
}
