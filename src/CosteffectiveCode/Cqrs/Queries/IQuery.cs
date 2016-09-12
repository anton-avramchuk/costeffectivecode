using JetBrains.Annotations;

namespace CostEffectiveCode.Cqrs.Queries
{
    [PublicAPI]
    public interface IQuery<in TSpecification, out TResult>
    {
        TResult Execute([NotNull] TSpecification specification);
    }
}
