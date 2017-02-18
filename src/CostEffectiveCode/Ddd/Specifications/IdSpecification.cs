﻿using System;
using CostEffectiveCode.Ddd.Entities;
using JetBrains.Annotations;

namespace CostEffectiveCode.Ddd.Specifications
{
    [PublicAPI]
    public class IdSpecification<TKey,T> : ExpressionSpecification<T>
        where TKey: IComparable, IComparable<TKey>, IEquatable<TKey>
        where T : IHasId<TKey>
    {
        public TKey Id { get; private set; }

        public IdSpecification(TKey id)
            : base(x => x.Id.Equals(id))
        {
            Id = id;
        }
    }
}
