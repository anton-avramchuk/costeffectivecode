using System;
using CostEffectiveCode.Ddd.Entities;
using JetBrains.Annotations;

namespace CostEffectiveCode.Ddd.Specifications.UnitOfWork
{
    [PublicAPI]
    public interface IUnitOfWork: IDisposable
    {
        void Add<TEntity>(TEntity entity)
            where TEntity : class, IEntity;

        void Delete<TEntity>(TEntity entity)
            where TEntity : class, IEntity;

        TEntity Get<TEntity>(object id);

        IEntity Get(Type entityType, object id);

        void Commit();
    }

}