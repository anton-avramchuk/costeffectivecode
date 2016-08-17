using JetBrains.Annotations;

namespace CosteffectiveCode.Cqrs.Queries
{
    [PublicAPI]
    public interface IQuery<in TSpecification, out TResult>
    {
        TResult Execute([NotNull] TSpecification specification);
    }
}
