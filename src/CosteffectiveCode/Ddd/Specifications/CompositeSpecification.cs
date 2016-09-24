using System;
using CostEffectiveCode.Ddd.Entities;
using JetBrains.Annotations;

namespace CostEffectiveCode.Ddd.Specifications
{
    public abstract class CompositeSpecification<T> : ISpecification<T>
        where T:IHasId
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
        where T : IHasId
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
        where T : IHasId
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
        where T : IHasId
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

}
