using JetBrains.Annotations;

namespace CostEffectiveCode.Cqrs.Queries
{
    [PublicAPI]
    public interface IQuery<out TOutput>
    {
        TOutput Execute();
    }

    [PublicAPI]
    public interface IQuery<in TSpecification, out TOutput>
    {
        TOutput Execute([NotNull] TSpecification specification);
    }
}
