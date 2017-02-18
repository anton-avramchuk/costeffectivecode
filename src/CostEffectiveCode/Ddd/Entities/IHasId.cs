﻿using System;

namespace CostEffectiveCode.Ddd.Entities
{
    public interface IHasId
    {
        object Id { get; }
    }

    public interface IHasId<out TKey> : IHasId
        where TKey: IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        new TKey Id { get; }
    }
}
