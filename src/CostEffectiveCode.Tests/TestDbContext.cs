using CostEffectiveCode.Ddd.Entities;
using CostEffectiveCode.Ddd.UnitOfWork;
using System;
using System.Data.Entity;
using System.Linq;


namespace CostEffectiveCode.Tests
{
    public class TestDbContext : DbContext, ILinqProvider
    {
        public TestDbContext() : base("DefaultConnection")
        {
            
        }

        public IDbSet<Category> Categories { get; set; }

        public IDbSet<Product> Products { get; set; }
        public IQueryable<TEntity> GetQueryable<TEntity>() where TEntity : class, IHasId
        {
            return Set<TEntity>();
        }

        public IQueryable GetQueryable(Type t)
        {
            return Set(t);
        }
    }
}
