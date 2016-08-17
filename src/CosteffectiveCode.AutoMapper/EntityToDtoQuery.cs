using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper.QueryableExtensions;
using CosteffectiveCode.Cqrs.Queries;
using CosteffectiveCode.Ddd.Entities;
using CosteffectiveCode.Ddd.Specifications;
using JetBrains.Annotations;
using AM = AutoMapper;

namespace CosteffectiveCode.AutoMapper
{
    public class PagedEntityToDtoQuery<TEntity, TResult> :
        PagedEntityToDtoQuery<PagedExpressionSpecification<TResult>, TEntity, TResult>
        where TEntity : class, IEntity
    {
        public PagedEntityToDtoQuery([NotNull] IQueryable<TEntity> queryable) : base(queryable)
        {
        }

        protected override IQueryable<TResult> GetQueryable(PagedExpressionSpecification<TResult> specification)
            => Project(Queryable)
                    .Where(specification.Expression)
                    .Skip(specification.Page * specification.Take)
                    .Take(specification.Take);
    }

    public abstract class PagedEntityToDtoQuery<TSpecification, TEntity, TDto> :
        EntityToDtoQuery<TSpecification, TEntity, TDto>
        where TEntity : class, IEntity
        where TSpecification: IPagedSpecification<TDto>
    {
        protected PagedEntityToDtoQuery([NotNull] IQueryable<TEntity> queryable) : base(queryable)
        {
        }
    }

    public class EntityToDtoQuery<TEntity, TResult> : EntityToDtoQuery<Expression<Func<TResult, bool>>, TEntity, TResult>
        where TEntity : class, IEntity
    {
        public EntityToDtoQuery([NotNull] IQueryable<TEntity> queryable) : base(queryable)
        {
        }

        protected override IQueryable<TResult> GetQueryable(Expression<Func<TResult, bool>> specification)
            => Project(Queryable).Where(specification);
    }

    public abstract class EntityToDtoQuery<TSpecification, TEntity, TDto>
        : IQuery<TSpecification, TDto[]>, IQuery<TSpecification, int>
        where TEntity : class , IEntity
    {
        protected readonly IQueryable<TEntity> Queryable;

        protected static void RegisterMapping(Action<AM.IMapperConfigurationExpression> registration)
        {
            AutoMapperWrapper.TypeMap
                .GetOrAdd(typeof(TEntity),
                    x => new ConcurrentDictionary<Type, Action<AM.IMapperConfigurationExpression>>())
                .GetOrAdd(typeof(TDto), x => registration);
        }

        protected EntityToDtoQuery([NotNull] IQueryable<TEntity> queryable)
        {
            if (queryable == null) throw new ArgumentNullException(nameof(queryable));
            Queryable = queryable;
        }

        protected abstract IQueryable<TDto> GetQueryable(TSpecification specification);

        protected IQueryable<TDto> Project(IQueryable<TEntity> queryable) => queryable.ProjectTo<TDto>();
        
        public TDto[] Execute(TSpecification specification) => GetQueryable(specification).ToArray();

        int IQuery<TSpecification, int>.Execute(TSpecification specification) => GetQueryable(specification).Count();
    }
}
