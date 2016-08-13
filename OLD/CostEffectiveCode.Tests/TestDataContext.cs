using System;
using System.Collections;
using System.Collections.Generic;
using CostEffectiveCode.Domain.Ddd.Entities;
using CostEffectiveCode.Domain.Ddd.UnitOfWork;
using CostEffectiveCode.Extensions;
using CostEffectiveCode.Sample.Domain.Entities;

namespace CostEffectiveCode.Tests
{
    public class TestDataContext : IUnitOfWork
    {
        public readonly Category Category;
        public readonly TestLinqProvider Provider;

        public TestDataContext()
        {
            Category = new Category("Toys") { Id = 1 };
            TestStorage = new Dictionary<Type, IEnumerable>
            {
                {
                    typeof (Product), new List<Product>
                    {
                        new Product("Teddy Bear", Category, 100) {Id = 1},
                        new Product("Transformer", Category, 100) {Id = 2},
                        new Product("Car", Category, 300, false) {Id = 3}
                    }
                }
            };
            Provider = new TestLinqProvider(TestStorage);
        }

        public Dictionary<Type, IEnumerable> TestStorage { get; set; }

        public void Dispose()
        {
            // pass-through
        }

        public void Commit()
        {
            // pass-through
        }

        public void Add<TEntity>(TEntity entity)
            where TEntity : class, IEntity
        {
            (TestStorage[typeof(TEntity)] as List<TEntity>)
                .CheckNotNull()
                .Add(entity);
        }

        public void Delete<TEntity>(TEntity entity)
            where TEntity : class, IEntity
        {
            // Provider.DeleteValue(entitys);
        }
    }
}
