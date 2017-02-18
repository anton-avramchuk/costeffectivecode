using System;
using CostEffectiveCode.Common;
using CostEffectiveCode.Ddd;
using CostEffectiveCode.Ddd.Entities;

namespace CostEffectiveCode.Cqrs.Commands
{
    public class CreateOrUpdateEntityHandler<TKey, TDto, TEntity>: IHandler<TDto, TKey>
        where TKey: IComparable, IComparable<TKey>, IEquatable<TKey>
        where TEntity : HasIdBase<TKey>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateOrUpdateEntityHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            if (unitOfWork == null) throw new ArgumentNullException(nameof(unitOfWork));
            if (mapper == null) throw new ArgumentNullException(nameof(mapper));

            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public TKey Handle(TDto context)
        {
            var id = (context as IHasId)?.Id;
            var entity = id != null && default(TKey)?.Equals(id) == false
                ? _mapper.Map(context, _unitOfWork.Find<TEntity>(id))
                : _mapper.Map<TEntity>(context);

            if (entity.IsNew())
            {
                _unitOfWork.Add(entity);
            }

            _unitOfWork.Commit();
            return entity.Id;
        }
    }
}
