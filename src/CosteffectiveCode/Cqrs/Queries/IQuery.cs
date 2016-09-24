using JetBrains.Annotations;

namespace CostEffectiveCode.Cqrs.Queries
{
    [PublicAPI]
    public interface IQuery<out TOutput>
    {
        TOutput Ask();
    }

    [PublicAPI]
    public interface IQuery<in TSpecification, out TOutput>
    {
        TOutput Ask([NotNull] TSpecification spec);
    }
}
