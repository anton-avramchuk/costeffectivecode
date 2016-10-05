using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CostEffectiveCode.Ddd;
using CostEffectiveCode.Ddd.Entities;

namespace WebApplication
{
    public class FakeContext : ILinqProvider, IUnitOfWork
    {
        private readonly Dictionary<Type, IQueryable> _queyables;

        public FakeContext(params IEnumerable[] enumerals)
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
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Add<TEntity>(TEntity entity) where TEntity : class, IHasId
        {
            throw new NotImplementedException();
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class, IHasId
        {
            throw new NotImplementedException();
        }

        public TEntity Find<TEntity>(object id) where TEntity : class, IHasId
        {
            throw new NotImplementedException();
        }

        public IHasId Find(Type entityType, object id)
        {
            throw new NotImplementedException();
        }

        public void Commit()
        {
            throw new NotImplementedException();
        }
    }
}
