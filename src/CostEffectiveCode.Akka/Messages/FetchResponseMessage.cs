using System;
using System.Collections.Generic;
using CostEffectiveCode.Domain.Ddd.Entities;
using JetBrains.Annotations;

namespace CostEffectiveCode.Akka.Messages
{
    public class FetchResponseMessage<TEntity>
        where TEntity: class, IEntity
    {
        public IEnumerable<TEntity> Entities { get; }

        public FetchResponseMessage([NotNull] TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            Entities = new List<TEntity> { entity };
        }

        public FetchResponseMessage([NotNull] IEnumerable<TEntity> entities)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));
            Entities = entities;
        }
    }
}
