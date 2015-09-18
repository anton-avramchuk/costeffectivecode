using System;
using System.Linq.Expressions;
using CostEffectiveCode.Domain.Ddd.Entities;
using JetBrains.Annotations;

namespace CostEffectiveCode.Domain.Ddd.Specifications
{
    [PublicAPI]
    public class ExpressionSpecification<T> : IExpressionSpecification<T>
        where T:class,IEntity
    {
        public Expression<Func<T, bool>> Expression { get; private set; }

        private Func<T, bool> _func;

        private Func<T, bool> Func
        {
            get
            {
                if (_func == null)
                {
                    _func = Expression.Compile();
                }

                return _func;
            }
        }

        public ExpressionSpecification([NotNull] Expression<Func<T, bool>> expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");
            Expression = expression;
        }

        public bool IsSatisfiedBy(T o)
        {
            return Func(o);
        }
    } 
}
