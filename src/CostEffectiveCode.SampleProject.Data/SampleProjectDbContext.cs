using CostEffectiveCode.EntityFramework7;
using CostEffectiveCode.SampleProject.Domain.Entities;
using Microsoft.Data.Entity;

namespace CostEffectiveCode.SampleProject.Data
{
    public class SampleProjectDbContext : IdentityDbContextBase<ApplicationUser>
    {
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
