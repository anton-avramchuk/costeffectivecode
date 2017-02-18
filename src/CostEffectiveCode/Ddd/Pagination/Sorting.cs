﻿using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace CostEffectiveCode.Ddd.Pagination
{
    public enum SortOrder
    {
        [PublicAPI] Asc = 1,
        [PublicAPI] Desc = 2
    }

    [PublicAPI]
    public class Sorting<TEntity, TKey>
        where TEntity: class
    {
        public Expression<Func<TEntity, TKey>> Expression { get; private set; }

        public SortOrder SortOrder { get; private set; }

        public Sorting(
            Expression<Func<TEntity, TKey>> expression,
            SortOrder sortOrder = SortOrder.Asc)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));
            Expression = expression;
            SortOrder = sortOrder;
        }
    }
}
