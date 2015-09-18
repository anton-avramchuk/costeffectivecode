namespace CostEffectiveCode.Domain.Cqrs.Queries
{
    public interface ISpecificQuery<out TResult, in TRequest> : ISpecificQuery
    {
        TResult Execute(TRequest @params);
    }

    public interface ISpecificQuery
    {
        
    }
}