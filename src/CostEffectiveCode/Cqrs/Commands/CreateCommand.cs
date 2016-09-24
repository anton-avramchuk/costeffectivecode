using System;
using CostEffectiveCode.Common;
using CostEffectiveCode.Ddd.Entities;
using CostEffectiveCode.Ddd.Specifications.UnitOfWork;
using JetBrains.Annotations;

namespace CostEffectiveCode.Cqrs.Commands
{
    public class CreateCommand<TKey, TDto, TEntity> : UowBased, ICommand<TDto, TKey>
        where TKey: struct
        where TEntity : HasIdBase<TKey>
    {
        private readonly IMapper _mapper;

        public CreateCommand([NotNull] IUnitOfWork unitOfWork, [NotNull] IMapper mapper) : base(unitOfWork)
        {
            if (mapper == null) throw new ArgumentNullException(nameof(mapper));
            _mapper = mapper;
        }

        public TKey Execute(TDto context)
        {
            var entity = _mapper.Map<TEntity>(context);
            UnitOfWork.Add(entity);
            UnitOfWork.Commit();
            return entity.Id;
        }

    }
}
