﻿using System;
using JetBrains.Annotations;

namespace CostEffectiveCode.Cqrs
{
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class MessageAttribute : Attribute, ITypeAssociation
    {
        public Type EntityType { get; }

        protected MessageAttribute(Type entityType)
        {
            if (entityType == null) throw new ArgumentNullException(nameof(entityType));
            EntityType = entityType;
        }
    }

    public class CommandAttribute : MessageAttribute
    {
        public CommandAttribute(Type entityType) : base(entityType)
        {
        }
    }

    public class DomainEventAttribute : MessageAttribute
    {
        public DomainEventAttribute(Type entityType) : base(entityType)
        {
        }
    }
}