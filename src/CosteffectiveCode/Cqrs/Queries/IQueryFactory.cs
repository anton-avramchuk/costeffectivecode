using JetBrains.Annotations;

namespace CostEffectiveCode.Cqrs.Queries
{
    [PublicAPI]
    public interface IQueryFactory
    {
        IQuery<TSpecification, TResult> GetQuery<TSpecification, TResult>();

        IQuery<TResult> GetQuery<TResult>();

        TQuery GetSpecificQuery<TResult, TQuery>()
            where TQuery : IQuery<TResult>;    

        TQuery GetSpecificQuery<TSpecification, TResult, TQuery>()
            where TQuery : IQuery<TSpecification, TResult>;
    }
}