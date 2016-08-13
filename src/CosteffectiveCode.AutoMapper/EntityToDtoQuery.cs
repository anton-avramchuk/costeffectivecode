using System.Linq;
using AutoMapper.QueryableExtensions;
using CostEffectiveCode.Domain.Cqrs.Queries;
using CostEffectiveCode.Domain.Ddd.Entities;
using JetBrains.Annotations;
using AM = AutoMapper;

namespace CosteffectiveCode.AutoMapper
{
    public class EntityToDtoQuery<TEntity, TResult> : ExpressionQueryBase<TEntity, TResult>
        where TEntity : class , IEntity
    {
        public EntityToDtoQuery([NotNull] IQueryable<TEntity> queryable) : base(queryable)
        {
        }

        protected override IQueryable<TResult> Project(IQueryable<TEntity> queryable) => queryable.ProjectTo<TResult>();
    }
}
