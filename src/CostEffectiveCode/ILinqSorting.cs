using System.Linq;

namespace CostEffectiveCode
{
    public interface ILinqSorting<T>
    {
        IOrderedQueryable<T> Apply(IQueryable<T> queryable);
    }
}