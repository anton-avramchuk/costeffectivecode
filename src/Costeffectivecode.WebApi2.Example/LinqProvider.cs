using System;
using System.Collections.Generic;
using System.Linq;
using CostEffectiveCode.Ddd.Entities;
using CostEffectiveCode.Ddd.Specifications.UnitOfWork;

namespace Costeffectivecode.WebApi2.Example
{
    public class LinqProvider : ILinqProvider
    {
        private readonly IDictionary<Type, object[]> _dictionary;

        public LinqProvider(params object[] values)
        {
            _dictionary = values
                .GroupBy(x => x.GetType())
                .ToDictionary(k => k.Key, v => v.ToArray());
        }

        public IQueryable<TEntity> GetQueryable<TEntity>() where TEntity : class, IEntity
            => _dictionary[typeof(TEntity)].Cast<TEntity>().AsQueryable();
    }
}