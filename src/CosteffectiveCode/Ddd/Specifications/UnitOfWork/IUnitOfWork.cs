using System;
using CosteffectiveCode.Ddd.Entities;
using JetBrains.Annotations;

namespace CosteffectiveCode.Ddd.Specifications.UnitOfWork
{
    [PublicAPI]
    public interface IUnitOfWork: IDisposable
    {
        void Add<TEntity>(TEntity entity)
            where TEntity : class, IEntity;

        void Delete<TEntity>(TEntity entity)
            where TEntity : class, IEntity;

        void Commit();
    }

}