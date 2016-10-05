using CostEffectiveCode.Ddd.Entities;

namespace WebApplication.Domain
{
    public class Category : HasIdBase<int>
    {
        public Category()
        {
            
        }

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
        public Product()
        {
            
        }

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
}
