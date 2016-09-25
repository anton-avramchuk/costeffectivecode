using System;
using CostEffectiveCode.Ddd.Entities;
using JetBrains.Annotations;

namespace CostEffectiveCode.Ddd.UnitOfWork
{
    [PublicAPI]
    public interface IUnitOfWork: IDisposable
    {
        void Add<TEntity>(TEntity entity)
            where TEntity : class, IHasId;

        void Delete<TEntity>(TEntity entity)
            where TEntity : class, IHasId;

        TEntity Find<TEntity>(object id);

        IHasId Find(Type entityType, object id);

        void Commit();
    }

}