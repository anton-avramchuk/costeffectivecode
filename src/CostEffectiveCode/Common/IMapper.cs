using System.Linq;
using JetBrains.Annotations;

namespace CostEffectiveCode.Common
{
    [PublicAPI]
    public interface IMapper
    {
        TReturn Map<TReturn>(object src);

        TReturn Map<TReturn>(object src, TReturn dest);

        IQueryable<TReturn> Project<TSource, TReturn>(IQueryable<TSource> queryable);
    }
}