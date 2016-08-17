using System.Linq;
using CosteffectiveCode.Ddd.Entities;
using JetBrains.Annotations;

namespace CosteffectiveCode.Ddd.Specifications.UnitOfWork
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

    }
}
