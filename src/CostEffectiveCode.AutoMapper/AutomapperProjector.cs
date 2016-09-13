using System.Linq;
using AutoMapper.QueryableExtensions;
using CostEffectiveCode.Common;

namespace CostEffectiveCode.AutoMapper
{
    public class AutomapperProjector : IProjector
    {
        public IQueryable<TReturn> Project<TSource, TReturn>(IQueryable<TSource> queryable)
            => queryable.ProjectTo<TReturn>();
    }
}
