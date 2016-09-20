using System.Linq;
using CostEffectiveCode.Ddd.Entities;

namespace CostEffectiveCode.Cqrs.Queries
{
    public interface ILinqSpecification<T>
        where T : IEntity
    {
        IQueryable<T> Apply(IQueryable<T> query);
    }
}
