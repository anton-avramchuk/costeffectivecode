using System.Data.Entity;
using CostEffectiveCode.EntityFramework6;
using CostEffectiveCode.Sample.Domain.Entities;

namespace CostEffectiveCode.SampleProject.Data.EntityFramework6 
{
    public class SampleDbContext : DbContextBase
    {
        public DbSet<Product> Products;

        public DbSet<Category> Categories;
    }
}
