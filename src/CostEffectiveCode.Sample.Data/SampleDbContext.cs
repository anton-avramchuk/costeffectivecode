using System.Data.Entity;
using CostEffectiveCode.EntityFramework6;
using CostEffectiveCode.Sample.Domain.Entities;

namespace CostEffectiveCode.Sample.Data 
{
    public class SampleDbContext : DbContextBase
    {
        public DbSet<Product> Products;

        public DbSet<Category> Categories;
    }
}
