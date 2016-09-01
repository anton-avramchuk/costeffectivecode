using System;
using System.Web.Http.Results;
using CostEffectiveCode.AutoMapper;
using CostEffectiveCode.Cqrs.Queries;
using CostEffectiveCode.Ddd.Entities;
using CostEffectiveCode.Ddd.Specifications;

namespace CostEffectiveCode.WebApi2
{
    public static class Extensions
    {
        public static OkNegotiatedContentResult<TResult> Ok<TSpecification, TResult, TQuery>(
            this CqrsController controller,
            TSpecification specification,
            Func<TSpecification, TQuery, TResult> func)
            where TQuery : IQuery<TSpecification, TResult>
            => new OkNegotiatedContentResult<TResult>(func.Invoke(specification, controller
                .QueryFactory
                .GetQuery<TSpecification, TResult, TQuery>()), controller);

        public static OkNegotiatedContentResult<TResult> Ok<TSpecification, TResult, TQuery>(
            this CqrsController controller,
            TSpecification specification)
            where TQuery : IQuery<TSpecification, TResult>
            => Ok<TSpecification, TResult, TQuery>(controller, specification, (spec, query) => query.Execute(spec));

        public static OkNegotiatedContentResult<TResult> Get<TKey, TEntity, TResult>(
            this CqrsController controller,
            TKey specification)
            where TKey: struct 
            where TEntity: class, IEntityBase<TKey>
            where TResult: IEntityBase<TKey>
            => Ok <TKey, TResult, GetQuery<TKey, TEntity,TResult>>(controller, specification);

        public static OkNegotiatedContentResult<TDto[]> List<TPagedSpecification, TEntity, TDto, TQuery>(
            this CqrsController controller,
            TPagedSpecification specification)
            where TPagedSpecification : IPagedSpecification<TDto>
            where TEntity : class, IEntity
            where TDto : IEntity
            where TQuery: PagedEntityToDtoQuery<TPagedSpecification, TEntity, TDto>
            => Ok<TPagedSpecification, TDto[], TQuery>(controller, specification);
    }
}
