using System;
using System.Linq;
using CostEffectiveCode.Domain.Ddd.Entities;
using Microsoft.Data.Entity;

namespace CostEffectiveCode.EntityFramework7
{
    public static class DbContextExtensions
    {
        public static void ExtendCommit(this DbContext context)
        {
            context.SaveChanges();
        }

        public static void ExtendAdd<TEntity>(this DbContext context, TEntity entity) where TEntity : class, IEntity
        {
            context.Set<TEntity>().Add(entity);
        }

        public static void ExtendDelete<TEntity>(this DbContext context, TEntity entity) where TEntity : class, IEntity
        {
            context.Set<TEntity>().Remove(entity);
        }

        public static IQueryable<TEntity> ExtendQuery<TEntity>(this DbContext context) where TEntity : class, IEntity
        {
            return context.Set<TEntity>();
        }

        public static TEntity ExtendFindById<TEntity, TPrimaryKey>(this DbContext context, TPrimaryKey id)
            where TEntity : class, IEntityBase<TPrimaryKey>
            where TPrimaryKey : struct, IComparable<TPrimaryKey>
        {
            return context.Set<TEntity>().SingleOrDefault(x => x.Id.CompareTo(id) == 0);
        }
    }
}
