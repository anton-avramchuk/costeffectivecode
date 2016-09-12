using System;
using System.Linq;
using CostEffectiveCode.Common;
using CostEffectiveCode.Ddd.Entities;
using CostEffectiveCode.Ddd.Specifications.UnitOfWork;
using JetBrains.Annotations;

namespace CostEffectiveCode.Cqrs.Queries
{
    public class GetQuery<TKey, TEntity, TResult> : IQuery<TKey, TResult>
        where TKey : struct
        where TEntity : class, IEntityBase<TKey>
        where TResult : IEntityBase<TKey>
    {
        private readonly ILinqProvider _linqProvider;

        private readonly IMapper _mapper;

        public GetQuery([NotNull] ILinqProvider linqProvider, [NotNull] IMapper mapper)
        {
            if (linqProvider == null) throw new ArgumentNullException(nameof(linqProvider));
            if (mapper == null) throw new ArgumentNullException(nameof(mapper));
            _linqProvider = linqProvider;
            _mapper = mapper;
        }

        public TResult Execute(TKey specification) =>
            _mapper.Project<TEntity, TResult>(_linqProvider
                .GetQueryable<TEntity>())
                .SingleOrDefault(x => x.Id.Equals(specification));
    }
}
