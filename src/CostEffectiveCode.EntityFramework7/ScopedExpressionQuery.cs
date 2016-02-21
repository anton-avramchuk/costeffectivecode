using System;
using System.Linq.Expressions;
using CostEffectiveCode.Domain.Cqrs.Queries;
using CostEffectiveCode.Domain.Ddd.Entities;
using CostEffectiveCode.Domain.Ddd.UnitOfWork;

namespace CostEffectiveCode.EntityFramework7
{
    public class ScopedExpressionQuery<TEntity, TLinqProvider> : ScopedExpressionQueryBase<TEntity, TLinqProvider>
        where TEntity : class, IEntity
        where TLinqProvider: ILinqProvider, IDisposable
    {
        public ScopedExpressionQuery(
            Func<TLinqProvider> linqProviderFactoryMethod,
            Expression<Func<TEntity, bool>> filter)
            : base(linqProviderFactoryMethod, filter, x => new ExpressionQuery<TEntity>(x))
        {
        }
    }
}
