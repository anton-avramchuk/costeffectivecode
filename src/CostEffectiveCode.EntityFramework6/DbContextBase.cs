using System;
using System.Data.Entity;
using System.Linq;
using CostEffectiveCode.Domain.Ddd.Entities;
using CostEffectiveCode.Domain.Ddd.UnitOfWork;

namespace CostEffectiveCode.EntityFramework
{
    public abstract class DbContextBase : DbContext, IDataContext
    {
        public IQueryable<TEntity> Query<TEntity>()
            where TEntity : class, IEntity
        {
            return this.ExtendQuery<TEntity>();
        }

        public void Add<TEntity>(TEntity entity)
            where TEntity : class, IEntity
        {
            this.ExtendAdd(entity);
        }

        public void Delete<TEntity>(TEntity entity)
            where TEntity : class, IEntity
        {
            this.ExtendDelete(entity);
        }

        public void Commit()
        {
            this.ExtendCommit();
        }

        public TEntity FindById<TEntity, TPrimaryKey>(TPrimaryKey id)
            where TEntity : class, IEntityBase<TPrimaryKey>
            where TPrimaryKey : struct, IComparable<TPrimaryKey>
        {
            return this.ExtendFindById<TEntity, TPrimaryKey>(id);
        }
    }
}
