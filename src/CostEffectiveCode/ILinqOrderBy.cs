using System.Linq;

namespace CostEffectiveCode
{
    public interface ILinqOrderBy<T>
    {
        IOrderedQueryable<T> Apply(IQueryable<T> queryable);
    }
}