using System.Linq;
using CostEffectiveCode.Common;
using CostEffectiveCode.Cqrs.Queries;
using CostEffectiveCode.Ddd.Entities;
using CostEffectiveCode.Ddd.Specifications.UnitOfWork;

namespace CostEffectiveCode.Components.Cqrs
{
    public class DictionaryQuery<TKey, TEntity, TDto> : IDictionaryQuery<TKey, TEntity, TDto>
        where TEntity : class, IHasId<TKey>
        where TDto : class, IHasId<TKey>
    {
        private readonly ILinqProvider _linqProvider;

        private readonly IProjector _projector;

        public DictionaryQuery(ILinqProvider linqProvider, IProjector projector)
        {
            _linqProvider = linqProvider;
            _projector = projector;
        }

        public TDto[] Execute()
            => _projector
                .Project<TEntity, TDto>(_linqProvider.GetQueryable<TEntity>())
                .ToArray();
    }

    public interface IDictionaryQuery<TKey, TEntity, TDto> : IQuery<TDto[]>
        where TEntity : class, IHasId<TKey>
        where TDto : class, IHasId<TKey>
    {
    }
}
