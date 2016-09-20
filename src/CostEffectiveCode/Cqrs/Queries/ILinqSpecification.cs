using System.Linq;

namespace CostEffectiveCode.Cqrs.Queries
{
    public interface ILinqSpecification<T>
        where T: class 
    {
        IQueryable<T> Apply(IQueryable<T> query);
    }
}
