using System;
using System.Linq;
using CostEffectiveCode.Ddd.Entities;
using JetBrains.Annotations;

namespace CostEffectiveCode.Ddd
{
    [PublicAPI]
    public interface ILinqProvider

    {
        IQueryable<TEntity> Query<TEntity>()
            where TEntity : class, IHasId;


        IQueryable Query(Type t);
    }
}