using CostEffectiveCode.EntityFramework7;
using CostEffectiveCode.SampleProject.Domain.Shared.Entities;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using ApplicationUser = CostEffectiveCode.SampleProject.Domain.AspNet5.Entities.ApplicationUser;

namespace CostEffectiveCode.SampleProject.Data.EntityFramework7
{
    public class SampleProjectDbContext : IdentityDbContextBase<ApplicationUser>
    {
        public SampleProjectDbContext()
            : base()
        {
        }

        public SampleProjectDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public SampleProjectDbContext(string connectionString)
            : base(GetOptions(connectionString))
        {
        }

        public static DbContextOptions GetOptions(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseSqlServer(connectionString);

            return optionsBuilder.Options;
        }

        public DbSet<Product> Products;

        public DbSet<Category> Categories;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
