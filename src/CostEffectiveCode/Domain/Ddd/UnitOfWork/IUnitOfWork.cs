using System;
using CosteffectiveCode.Domain.Ddd.Entities;
using JetBrains.Annotations;

namespace CosteffectiveCode.Domain.Ddd.UnitOfWork
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