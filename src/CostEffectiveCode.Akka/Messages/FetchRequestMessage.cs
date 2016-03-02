using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using CostEffectiveCode.Domain.Cqrs.Queries;
using CostEffectiveCode.Domain.Ddd.Entities;
using CostEffectiveCode.Domain.Ddd.Specifications;

namespace CostEffectiveCode.Akka.Messages
{
    public class FetchRequestMessage<TEntity, TSpecification> : IQueryConstraints<TEntity, TSpecification, FetchRequestMessage<TEntity, TSpecification>>
        where TEntity : class, IEntity
        where TSpecification : ISpecification<TEntity>
    {
        public FetchRequestMessage(bool single)
        {
            Single = single;
            Page = null;
            Limit = null;
        }

        public FetchRequestMessage(int page, int limit)
        {
            Single = false;
            Page = page;
            Limit = limit;
        }

        public FetchRequestMessage(int limit)
        {
            Single = false;
            Page = null;
            Limit = limit;
        }

        public bool Single { get; set; }

        public int? Page { get; set; }

        public int? Limit { get; set; }


        public ICollection<TSpecification> WhereSpecificationConstraints { get; protected set; }

        public IList OrderByConstraints { get; protected set; }

        public ICollection<LambdaExpression> IncludeConstraints { get; protected set; }

        public FetchRequestMessage<TEntity, TSpecification> Where(TSpecification specification)
        {
            if (specification == null) throw new ArgumentNullException(nameof(specification));

            WhereSpecificationConstraints.Add(specification);

            return this;
        }

        public FetchRequestMessage<TEntity, TSpecification> OrderBy<TProperty>(Expression<Func<TEntity, TProperty>> expression, SortOrder sortOrder = SortOrder.Asc)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            OrderByConstraints.Add(
                new OrderBySpecification<TEntity, TProperty>(expression, sortOrder));

            return this;
        }

        public FetchRequestMessage<TEntity, TSpecification> Include<TProperty>(Expression<Func<TEntity, TProperty>> expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            IncludeConstraints.Add(expression);

            return this;
        }
    }
}