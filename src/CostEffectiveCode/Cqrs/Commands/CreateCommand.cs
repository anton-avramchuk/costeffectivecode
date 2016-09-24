using System;
using CostEffectiveCode.Common;
using CostEffectiveCode.Ddd.Entities;
using CostEffectiveCode.Ddd.Specifications.UnitOfWork;
using JetBrains.Annotations;

namespace CostEffectiveCode.Cqrs.Commands
{
    public class CreateCommandHandler<TKey, TDto, TEntity> : UowBased, ICommandHandler<TDto, TKey>
        where TKey: struct
        where TEntity : HasIdBase<TKey>
    {
        private readonly IMapper _mapper;

        public CreateCommandHandler([NotNull] IUnitOfWork unitOfWork, [NotNull] IMapper mapper) : base(unitOfWork)
        {
            if (mapper == null) throw new ArgumentNullException(nameof(mapper));
            _mapper = mapper;
        }

        public TKey Handle(TDto context)
        {
            var entity = _mapper.Map<TEntity>(context);
            UnitOfWork.Add(entity);
            UnitOfWork.Commit();
            return entity.Id;
        }

    }
}
