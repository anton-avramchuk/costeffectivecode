using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CostEffectiveCode.Ddd.Entities;
using CostEffectiveCode.Ddd.Specifications.UnitOfWork;

namespace CostEffectiveCode.Tests
{
    public class InMemoryLinqProvider : ILinqProvider
    {
        private readonly Dictionary<Type, IQueryable> _queyables;

        public InMemoryLinqProvider(params IEnumerable[] enumerals)
        {
            _queyables = enumerals.ToDictionary(
                x =>
                {
                    var e = x.GetEnumerator();
                    e.MoveNext();
                    return e.Current.GetType();
                }, x => x.AsQueryable());
        }

        public IQueryable<TEntity> GetQueryable<TEntity>() where TEntity : class, IHasId
            => GetQueryable(typeof(TEntity)).Cast<TEntity>();   

        public IQueryable GetQueryable(Type t) => _queyables[t];
    }
}
