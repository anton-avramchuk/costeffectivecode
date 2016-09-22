using System;
using JetBrains.Annotations;

namespace CostEffectiveCode.Components.Cqrs
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DtoForAttribute : Attribute
    {
        public Type EntityType { get; }

        public DtoForAttribute([NotNull] Type entityType)
        {
            if (entityType == null) throw new ArgumentNullException(nameof(entityType));
            EntityType = entityType;
        }
    }
}
