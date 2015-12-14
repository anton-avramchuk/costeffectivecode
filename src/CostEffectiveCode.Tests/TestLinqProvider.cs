using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CostEffectiveCode.Domain.Ddd.Entities;
using CostEffectiveCode.Domain.Ddd.UnitOfWork;

namespace CostEffectiveCode.Tests
{
    public class TestLinqProvider : ILinqProvider
    {
        private readonly Dictionary<Type, IEnumerable> _source;

        public TestLinqProvider(Dictionary<Type, IEnumerable> testStorage)
        {
            _source = testStorage;
        }

        public IQueryable<T> Query<T>() where T : class, IEntity
        {
            if (!_source.ContainsKey(typeof(T)) || _source[typeof(T)] == null)
            {
                _source[typeof(T)] = new ArrayList();
            }

            return _source[typeof(T)]
                .Cast<T>()
                .AsQueryable<T>();
        }
    }
}
