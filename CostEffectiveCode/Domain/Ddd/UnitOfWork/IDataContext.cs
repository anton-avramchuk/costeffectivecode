using JetBrains.Annotations;

namespace CostEffectiveCode.Domain.Ddd.UnitOfWork
{
    [PublicAPI]
    public interface IDataContext : ILinqProvider, IUnitOfWork
    {
    }
}
