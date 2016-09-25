using System;
using System.Linq;
using CostEffectiveCode.Ddd.Entities;
using JetBrains.Annotations;

namespace CostEffectiveCode.Ddd.UnitOfWork
{
    [PublicAPI]
    public interface ILinqProvider

    {
        /// <summary>
        ///     Query object for concrete <see cref="IHasId" />
        /// </summary>
        /// <typeparam name="TEntity">
        ///     <see cref="IHasId" />
        /// </typeparam>
        IQueryable<TEntity> GetQueryable<TEntity>()
            where TEntity : class, IHasId;


        IQueryable GetQueryable(Type t);
    }
}
