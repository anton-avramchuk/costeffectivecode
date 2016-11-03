using System.Linq;

namespace CostEffectiveCode.Cqrs
{
    public interface ILinqSpecification<T>
        where T: class 
    {
        IQueryable<T> Where(IQueryable<T> query);
    }
}
