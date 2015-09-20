using System;
using System.Collections;
using System.Collections.Generic;
using CostEffectiveCode.Tests.Entities;

namespace CostEffectiveCode.Tests
{
    public class TestDataContext
    {
        public readonly Category Category;
        public readonly TestLinqProvider Provider;

        public TestDataContext()
        {
            Category = new Category("Toys") {Id = 1};
            Provider = new TestLinqProvider(new Dictionary<Type, IEnumerable>()
            {
                {
                    typeof (Product), new[]
                    {
                        new Product("Teddy Bear", Category, 100) {Id = 1},
                        new Product("Transformer", Category, 100) {Id = 2},
                        new Product("Car", Category, 300, false) {Id = 3}
                    }

                }
            });
        }
    }
}
