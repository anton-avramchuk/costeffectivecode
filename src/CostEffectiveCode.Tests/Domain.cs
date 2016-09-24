using CostEffectiveCode.Components.Cqrs;
using CostEffectiveCode.Ddd.Entities;

namespace CostEffectiveCode.Tests
{
    public class Category : HasIdBase<int>
    {
        public Category(int rating, string name)
        {
            Rating = rating;
            Name = name;
        }

        public int Rating { get; set; }

        public string Name { get; set; }
    }

    public class Product : HasIdBase<int>
    {
        public Product(Category category, string name, decimal price)
        {
            Category = category;
            Name = name;
            Price = price;
        }

        public Category Category { get; set; }
        public string Name { get; set; }

        public decimal Price { get; set; }
    }

    [DtoFor(typeof(Product))]
    public class ProductDto : HasIdBase<int>
    {
        public string CategoryName { get; set; }

        public decimal Price { get; set; }
    }
}
