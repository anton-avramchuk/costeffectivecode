using JetBrains.Annotations;

namespace CostEffectiveCode.Domain.Ddd.Entities
{
    [PublicAPI]
    public interface IEntity
    {
        string GetId();
    }
}