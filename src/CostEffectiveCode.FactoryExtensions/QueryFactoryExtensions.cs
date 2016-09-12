namespace CostEffectiveCode.FactoryExtensions
{
    public static class QueryFactoryExtensions
    {
        public static TResult Execute<TSpecification, TResult, TQuery>()
        {
            
        }

        public OkNegotiatedContentResult<TResult> Ok<TSpecification, TResult, TQuery>(
            TSpecification specification)
            where TQuery : IQuery<TSpecification, TResult>
        => Ok(QueryFactory.GetQuery<TSpecification, TResult, TQuery>().Execute(specification));

        public OkNegotiatedContentResult<TResult> Get<TKey, TEntity, TResult>(
            TKey specification)
            where TKey : struct
            where TEntity : class, IEntityBase<TKey>
            where TResult : IEntityBase<TKey>
        => Ok<TKey, TResult, GetQuery<TKey, TEntity, TResult>>(specification);

        public OkNegotiatedContentResult<TDto[]> List<TPagedSpecification, TEntity, TDto, TQuery>(
            TPagedSpecification specification)
            where TPagedSpecification : IPagedSpecification<TDto>
            where TEntity : class, IEntity
            where TDto : IEntity
            where TQuery : PagedEntityToDtoQuery<TPagedSpecification, TEntity, TDto>
        => Ok<TPagedSpecification, TDto[], TQuery>(specification);
    }
}
