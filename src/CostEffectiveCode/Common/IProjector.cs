using System.Linq;
using JetBrains.Annotations;

namespace CostEffectiveCode.Common
{
    [PublicAPI]
    public interface IProjector
    {
        IQueryable<TReturn> Project<TReturn>(IQueryable queryable);
    }
}
