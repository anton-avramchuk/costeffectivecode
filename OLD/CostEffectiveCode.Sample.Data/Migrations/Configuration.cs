using CostEffectiveCode.Sample.Domain.Entities;
using System.Data.Entity.Migrations;

namespace CostEffectiveCode.Sample.Data.Migrations
{

    internal sealed class Configuration : DbMigrationsConfiguration<SampleDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SampleDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );

            var carsCategory = new Category("Cars");
            var toysCategory = new Category("Toys");

            context.Categories.AddOrUpdate(x => x.Name, carsCategory, toysCategory);

            context.Products.AddOrUpdate(x => x.Name, new Product("Ferrari", carsCategory, 1000));
            context.Products.AddOrUpdate(x => x.Name, new Product("Mazeratti", carsCategory, 2000));
            context.Products.AddOrUpdate(x => x.Name, new Product("Lada", carsCategory, 5));

            context.Products.AddOrUpdate(x => x.Name, new Product("PS3", toysCategory, 100));
            context.Products.AddOrUpdate(x => x.Name, new Product("Xbox", toysCategory, 200));
            context.Products.AddOrUpdate(x => x.Name, new Product("PC", toysCategory, 300));

        }
    }
}
