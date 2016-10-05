using System;
using JetBrains.Annotations;

namespace CostEffectiveCode.AutoMapper
{
    public enum MapDirection
    {
        EntityToDto, DtoToEntity, Both
    }

    public class ConventionalMapAttribute : Attribute
    {
        public MapDirection Direction { get; set; }

        public Type EntityType { get; }

        public ConventionalMapAttribute([NotNull] Type entityType, MapDirection direction = MapDirection.Both)
        {
            if (entityType == null) throw new ArgumentNullException(nameof(entityType));
            Direction = direction;
            EntityType = entityType;
        }
    }
}
