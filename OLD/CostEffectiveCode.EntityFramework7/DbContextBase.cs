using System;
using System.Linq;
using CostEffectiveCode.Domain.Ddd.Entities;
using CostEffectiveCode.Domain.Ddd.UnitOfWork;
using JetBrains.Annotations;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;

namespace CostEffectiveCode.EntityFramework7
{
    public abstract class DbContextBase : DbContext, IDataContext
    {
        #region ctors
        protected DbContextBase()
        {
        }

        protected DbContextBase([NotNull] IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected DbContextBase([NotNull] DbContextOptions options) : base(options)
        {
        }

        protected DbContextBase([NotNull] IServiceProvider serviceProvider, [NotNull] DbContextOptions options) : base(serviceProvider, options)
        {
        }
        #endregion

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
