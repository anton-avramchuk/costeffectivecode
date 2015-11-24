using System;
using CostEffectiveCode.Domain.Ddd.Entities;
using JetBrains.Annotations;

namespace CostEffectiveCode.Domain.Ddd.UnitOfWork
{
    [PublicAPI]
    public interface IDataContext : ILinqProvider, IUnitOfWork
    {
        [CanBeNull]
        T FindById<T, TPrimaryKey>(TPrimaryKey id)
            where T : IEntityBase<TPrimaryKey>;

        [CanBeNull]
        object FindById(Type type, object id);
    }
}
