using System;
using CosteffectiveCode.Domain.Ddd.Entities;
using JetBrains.Annotations;

namespace CosteffectiveCode.Domain.Ddd.UnitOfWork
{
    [PublicAPI]
    public interface IDataContext : ILinqProvider, IUnitOfWork
    {
        [CanBeNull]
        TEntity FindById<TEntity, TPrimaryKey>(TPrimaryKey id)
            where TEntity : class, IEntityBase<TPrimaryKey>
            where TPrimaryKey : struct, IComparable<TPrimaryKey>;

        //[CanBeNull]
        //object FindById(Type entityType, object id);
    }
}
