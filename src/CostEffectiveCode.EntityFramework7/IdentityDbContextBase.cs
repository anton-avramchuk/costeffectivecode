using System;
using System.Linq;
using CostEffectiveCode.Domain.Ddd.Entities;
using CostEffectiveCode.Domain.Ddd.UnitOfWork;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity.Infrastructure;

namespace CostEffectiveCode.EntityFramework7
{
    public class IdentityDbContextBase<TUser, TRole, TKey> : IdentityDbContext<TUser, TRole, TKey>, IDataContext
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
    {
        #region ctors
        public IdentityDbContextBase(DbContextOptions options) : base(options)
        {
        }

        public IdentityDbContextBase(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public IdentityDbContextBase(IServiceProvider serviceProvider, DbContextOptions options) : base(serviceProvider, options)
        {
        }

        protected IdentityDbContextBase()
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
