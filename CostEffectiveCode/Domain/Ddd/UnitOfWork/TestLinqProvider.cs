using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CostEffectiveCode.Domain.Ddd.Entities;

namespace CostEffectiveCode.Domain.Ddd.UnitOfWork
{
    public class TestLinqProvider : ILinqProvider
    {
        private readonly Dictionary<Type, ArrayList> _source;

        public TestLinqProvider(Dictionary<Type, IEnumerable> values)
        {
            _source = new Dictionary<Type, ArrayList>();
            // type checking
            foreach (var value in values)
            {
                _source[value.Key] = new ArrayList(value.Value
                    .Cast<object>()
                    .Select(v => Convert.ChangeType(v, value.Key))
                    .ToArray());
            }
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
