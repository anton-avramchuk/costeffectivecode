using System;
using System.Collections.Concurrent;
using System.Linq;
using AutoMapper.QueryableExtensions;
using CosteffectiveCode.Domain.Cqrs.Queries;
using CosteffectiveCode.Domain.Ddd.Entities;
using JetBrains.Annotations;
using AM = AutoMapper;

namespace CosteffectiveCode.AutoMapper
{
    public class EntityToDtoQuery<TEntity, TResult> : ExpressionQueryBase<TEntity, TResult>
        where TEntity : class , IEntity
    {
        protected static void RegisterMapping(Action<AM.IMapperConfigurationExpression> registration)
        {
            AutoMapperWrapper.TypeMap
                .GetOrAdd(typeof(TEntity),
                    x => new ConcurrentDictionary<Type, Action<AM.IMapperConfigurationExpression>>())
                .GetOrAdd(typeof(TResult), x => registration);
        }

        public EntityToDtoQuery([NotNull] IQueryable<TEntity> queryable)
            : base(queryable)
        {
        }

        protected override IQueryable<TResult> Project(IQueryable<TEntity> queryable) => queryable.ProjectTo<TResult>();
    }
}
