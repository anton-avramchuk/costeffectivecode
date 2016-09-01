using JetBrains.Annotations;

namespace CostEffectiveCode.Cqrs.Queries
{
    [PublicAPI]
    public interface IQueryFactory
    {
        IQuery<TSpecification, TResult> GetQuery<TSpecification, TResult>();

        TQuery GetQuery<TSpecification, TResult, TQuery>()
            where TQuery : IQuery<TSpecification, TResult>;
    }
}