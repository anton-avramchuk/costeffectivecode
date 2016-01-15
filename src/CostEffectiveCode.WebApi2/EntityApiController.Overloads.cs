using System;
using CostEffectiveCode.Common;
using CostEffectiveCode.Domain.Cqrs.Commands;
using CostEffectiveCode.Domain.Cqrs.Queries;
using CostEffectiveCode.Domain.Ddd.Entities;

namespace CostEffectiveCode.WebApi2
{
    public class EntityApiController<TEntity, TPrimaryKey, TViewModel> : EntityApiController<TEntity, TPrimaryKey, TViewModel, TViewModel>
        where TEntity : class, IEntityBase<TPrimaryKey>
        where TPrimaryKey : struct, IComparable<TPrimaryKey>
    {
        public EntityApiController(IQueryFactory queryFactory, ICommandFactory commandFactory, IMapper mapper, ILogger logger)
            : base(queryFactory, commandFactory, mapper, logger)
        {
        }

    }

    public class EntityApiController<TEntity, TPrimaryKey> : EntityApiController<TEntity, TPrimaryKey, TEntity>
        where TEntity : class, IEntityBase<TPrimaryKey>
        where TPrimaryKey : struct, IComparable<TPrimaryKey>
    {
        public EntityApiController(IQueryFactory queryFactory, ICommandFactory commandFactory, IMapper mapper, ILogger logger)
            : base(queryFactory, commandFactory, mapper, logger)
        {
        }

    }
}