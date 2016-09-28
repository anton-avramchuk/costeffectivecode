using System;
using System.Data.Entity;
using System.Linq;
using CostEffectiveCode.Ddd;
using CostEffectiveCode.Ddd.Entities;

namespace CostEffectiveCode.Tests.Stubs
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
