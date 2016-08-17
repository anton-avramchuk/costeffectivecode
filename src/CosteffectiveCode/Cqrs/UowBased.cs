using System;
using CosteffectiveCode.Ddd.Specifications.UnitOfWork;
using JetBrains.Annotations;

namespace CosteffectiveCode.Cqrs
{
    public abstract class UowBased
    {
        protected readonly IUnitOfWork UnitOfWork;

        protected UowBased([NotNull] IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null) throw new ArgumentNullException(nameof(unitOfWork));
            UnitOfWork = unitOfWork;
        }
    }
}
