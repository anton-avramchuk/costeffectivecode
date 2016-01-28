using System.Data.Entity;
using CostEffectiveCode.Sample.Domain.Entities;

namespace CostEffectiveCode.Sample.Data 
{
    public class SampleDbContext : DbContext
    {
        public SampleDbContext()
            : base("Name=ctx")
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
