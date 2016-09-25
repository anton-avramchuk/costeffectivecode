using System;
using CostEffectiveCode.Common;
using CostEffectiveCode.Ddd.Entities;
using CostEffectiveCode.Ddd.UnitOfWork;
using JetBrains.Annotations;

namespace CostEffectiveCode.Cqrs.Commands
{
    public class CreateOrUpdateHandler<TKey, TDto, TEntity> : UowBased, ICommandHandler<TDto, TKey>
        where TKey: struct
        where TEntity : HasIdBase<TKey>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateOrUpdateHandler(
            [NotNull] IUnitOfWork unitOfWork,
            [NotNull] IMapper mapper) : base(unitOfWork)
        {
            if (unitOfWork == null) throw new ArgumentNullException(nameof(unitOfWork));
            if (mapper == null) throw new ArgumentNullException(nameof(mapper));

            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public TKey Handle(TDto context)
        {
            var id = (context as IHasId)?.Id;
            var entity = !default(TKey).Equals(id)
                ? _mapper.Map(context, _unitOfWork.Find<TEntity>(id))
                : _mapper.Map<TEntity>(context);

            if (entity.IsNew())
            {
                UnitOfWork.Add(entity);
            }

            UnitOfWork.Commit();
            return entity.Id;
        }

    }
}
