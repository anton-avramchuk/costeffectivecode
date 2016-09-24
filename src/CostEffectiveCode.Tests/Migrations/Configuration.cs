using System.Collections.Generic;

namespace CostEffectiveCode.Tests.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CostEffectiveCode.Tests.TestDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TestDbContext context)
        {
            var catsList = new List<Category>();
            for (int i = 1; i < 1000; i++)
            {
                catsList.Add(new Category(i, i.ToString()) {Id = i});
            }

            var catsArray = catsList.ToArray();

            context.Categories.AddOrUpdate(catsArray);
            context.SaveChanges();

            catsArray = context.Categories.ToArray();

            var prs = new List<Product>();
            for (int i = 1; i < 1000; i++)
            {
                prs.Add(new Product(catsArray.FirstOrDefault(x => x.Id == i), i.ToString(), i * 100));
            }

            context.Products.AddOrUpdate(prs.ToArray());
            context.SaveChanges();


        }
    }
}
