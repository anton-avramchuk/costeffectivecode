using System;
using System.Linq.Expressions;
using CostEffectiveCode.Domain.Ddd.Entities;
using JetBrains.Annotations;

namespace CostEffectiveCode.Domain.Ddd.Specifications
{
    public abstract class CompositeSpecification<T> : ISpecification<T>
        where T:IEntity
    {
        protected readonly ISpecification<T> Left;
        protected readonly ISpecification<T> Right;

        protected CompositeSpecification([NotNull] ISpecification<T> left, [NotNull] ISpecification<T> right)
        {
            if (left == null) throw new ArgumentNullException(nameof(left));
            if (right == null) throw new ArgumentNullException(nameof(right));

            Left = left;
            Right = right;
        }

        public abstract bool IsSatisfiedBy(T o);
    }

    public class AndSpecification<T> : CompositeSpecification<T>
        where T : IEntity
    {
        public override bool IsSatisfiedBy(T o)
        {
            return Left.IsSatisfiedBy(o)
                && Right.IsSatisfiedBy(o);
        }

        public AndSpecification([NotNull] ISpecification<T> left, [NotNull] ISpecification<T> right)
            : base(left, right)
        {
        }
    }

    public class OrSpecification<T> : CompositeSpecification<T>
        where T : IEntity
    {
        public override bool IsSatisfiedBy(T o)
        {
            return Left.IsSatisfiedBy(o)
                || Right.IsSatisfiedBy(o);
        }

        public OrSpecification([NotNull] ISpecification<T> left, [NotNull] ISpecification<T> right)
            : base(left, right)
        {
        }
    }

    public class NotSpecification<T> : ISpecification<T>
        where T : IEntity
    {
        private readonly ISpecification<T> _specification;

        public NotSpecification(ISpecification<T> specification)
        {
            _specification = specification;
        }

        public bool IsSatisfiedBy(T o)
        {
            return !_specification.IsSatisfiedBy(o);
        }
    }

    public class ExpressionAndSpecification<T> : AndSpecification<T>, IExpressionSpecification<T>
        where T :class, IEntity
    {
        // Don't alt+enter with r# refactoring here
        public ExpressionAndSpecification(
            [NotNull] IExpressionSpecification<T> left,
            [NotNull] IExpressionSpecification<T> right)
            : base(left, right)
        {
        }


        public Expression<Func<T, bool>> Expression => System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(
            System.Linq.Expressions.Expression.And(
                ((IExpressionSpecification<T>) Left).Expression,
                ((IExpressionSpecification<T>) Right).Expression));
    }

    public class ExpressionOrSpecification<T> : OrSpecification<T>, IExpressionSpecification<T>
        where T : class, IEntity
    {
        // Don't alt+enter with r# refactoring here
        public ExpressionOrSpecification(
            [NotNull] IExpressionSpecification<T> left,
            [NotNull] IExpressionSpecification<T> right)
            : base(left, right)
        {
        }


        public Expression<Func<T, bool>> Expression => System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(
            System.Linq.Expressions.Expression.Or(
                ((IExpressionSpecification<T>)Left).Expression,
                ((IExpressionSpecification<T>)Right).Expression));
    }

}
