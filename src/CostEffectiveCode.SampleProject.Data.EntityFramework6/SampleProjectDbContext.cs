using System.Data.Entity;
using CostEffectiveCode.EntityFramework6;
using CostEffectiveCode.SampleProject.Domain.Shared.Entities;
using ApplicationUser = CostEffectiveCode.SampleProject.Domain.AspNet45.Entities.ApplicationUser;

namespace CostEffectiveCode.SampleProject.Data.EntityFramework6 
{
    public class SampleProjectDbContext : IdentityDbContextBase<ApplicationUser>
    {
        
        public DbSet<Product> Products;

        public DbSet<Category> Categories;

    }
}
