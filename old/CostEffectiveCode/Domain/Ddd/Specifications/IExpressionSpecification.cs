using System;
using System.Linq.Expressions;
using CostEffectiveCode.Domain.Ddd.Entities;
using JetBrains.Annotations;

namespace CostEffectiveCode.Domain.Ddd.Specifications
{
    [PublicAPI]
    public interface IExpressionSpecification<T> : ISpecification<T>
        where T:class,IEntity
    {
        Expression<Func<T, bool>> Expression { get; }
    }
}