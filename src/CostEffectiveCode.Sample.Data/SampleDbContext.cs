using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using CostEffectiveCode.EntityFramework6;
using CostEffectiveCode.Sample.Domain.Entities;

namespace CostEffectiveCode.Sample.Data 
{
    public class SampleDbContext : DbContextBase
    {
        public SampleDbContext()
            : base("Name=ctx")
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<SampleDbContext>());
        }

        public SampleDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<SampleDbContext>());
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
