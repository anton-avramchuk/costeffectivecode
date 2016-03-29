using System.Data.Entity;
using CostEffectiveCode.EntityFramework6;
using CostEffectiveCode.Sample.Domain.Entities;

namespace CostEffectiveCode.Sample.Data 
{
    public class SampleDbContext : DbContextBase
    {
        public SampleDbContext()
            : this("Name=ctx")
        {
        }

        public SampleDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<SampleDbContext>());

            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }
    }
}
