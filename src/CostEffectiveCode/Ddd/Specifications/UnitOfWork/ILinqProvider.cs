using System;
using System.Linq;
using CostEffectiveCode.Ddd.Entities;
using JetBrains.Annotations;

namespace CostEffectiveCode.Ddd.Specifications.UnitOfWork
{
    [PublicAPI]
    public interface ILinqProvider

    {
        /// <summary>
        ///     Query object for concrete <see cref="IEntity" />
        /// </summary>
        /// <typeparam name="TEntity">
        ///     <see cref="IEntity" />
        /// </typeparam>
        IQueryable<TEntity> GetQueryable<TEntity>()
            where TEntity : class, IEntity;


        IQueryable GetQueryable(Type t);
    }
}
