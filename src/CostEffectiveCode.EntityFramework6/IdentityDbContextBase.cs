using System;
using System.Linq;
using CostEffectiveCode.Domain.Ddd.Entities;
using CostEffectiveCode.Domain.Ddd.UnitOfWork;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CostEffectiveCode.EntityFramework6
{
    public class IdentityDbContextBase<TUser, TRole, TKey, TUserLogin, TUserRole, TUserClaim>
        : IdentityDbContext<TUser, TRole, TKey, TUserLogin, TUserRole, TUserClaim>, IDataContext
        where TUser : IdentityUser<TKey, TUserLogin, TUserRole, TUserClaim>
        where TRole : IdentityRole<TKey, TUserRole>
        where TUserLogin : IdentityUserLogin<TKey>
        where TUserRole : IdentityUserRole<TKey>
        where TUserClaim : IdentityUserClaim<TKey>
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
