using System.Linq;

namespace CostEffectiveCode.Cqrs
{
    public interface ILinqSpecification<T>
        where T: class 
    {
        IQueryable<T> Apply(IQueryable<T> query);
    }
}
