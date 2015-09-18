using System;
using CostEffectiveCode.Common;
using CostEffectiveCode.Domain.Cqrs.Commands;
using CostEffectiveCode.Domain.Ddd.UnitOfWork;
using JetBrains.Annotations;

namespace CostEffectiveCode.Domain.Cqrs
{
    public abstract class UnitOfWorkScopeCommand : ICommand
    {
        protected readonly IScope<IUnitOfWork> UnitOfWorkScope;
        public abstract void Execute();

        protected UnitOfWorkScopeCommand([NotNull] IScope<IUnitOfWork> unitOfWorkScopeScope)
        {
            if (unitOfWorkScopeScope == null) throw new ArgumentNullException("unitOfWorkScopeScope");
            UnitOfWorkScope = unitOfWorkScopeScope;
        }
    }

    public abstract class UnitOfWorkScopeCommand<T> : ICommand<T>
    {
        protected readonly IScope<IUnitOfWork> UnitOfWorkScope;

        public abstract void Execute(T context);

        protected UnitOfWorkScopeCommand([NotNull] IScope<IUnitOfWork> unitOfWorkScope)
        {
            if (unitOfWorkScope == null) throw new ArgumentNullException("unitOfWorkScope");
            UnitOfWorkScope = unitOfWorkScope;
        }
    }
}
