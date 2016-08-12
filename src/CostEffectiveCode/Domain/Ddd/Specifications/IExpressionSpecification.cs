using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace CostEffectiveCode.Domain.Ddd.Specifications
{
    [PublicAPI]
    public interface IExpressionSpecification<T> : ISpecification<T>
    {
        Expression<Func<T, bool>> Expression { get; }
    }
}