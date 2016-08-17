using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using CosteffectiveCode.Domain.Cqrs.Queries;
using CosteffectiveCode.Domain.Ddd.Entities;
using CosteffectiveCode.Domain.Ddd.Specifications;


namespace CostEffectiveCode.Akka.Messages
{
    public class FetchRequestMessage<TEntity, TSpecification>
        : FetchRequestMessageBase, ISpecificationQuery<TEntity, TSpecification, FetchRequestMessage<TEntity, TSpecification>>
        where TEntity : class, IEntity
        where TSpecification : ISpecification<TEntity>
    {
        public FetchRequestMessage(bool single, bool firstOrDefault) : base(single, firstOrDefault)
        {
        }

        public FetchRequestMessage(int page, int limit) : base(page, limit)
        {
        }

        public FetchRequestMessage(int limit) : base(limit)
        {
        }

        public FetchRequestMessage() : base()
        {
        }

        public ICollection<TSpecification> WhereSpecificationConstraints { get; protected set; }
            = new List<TSpecification>();

        public IList OrderByConstraints { get; protected set; }
            = new ArrayList();

        public ICollection<LambdaExpression> IncludeConstraints { get; protected set; }
            = new List<LambdaExpression>();

        public FetchRequestMessage<TEntity, TSpecification> Where(TSpecification specification)
        {
            if (specification == null) throw new ArgumentNullException(nameof(specification));

            WhereSpecificationConstraints.Add(specification);

            return this;
        }

        //public FetchRequestMessage<TEntity, TSpecification> OrderBy<TProperty>(Expression<Func<TEntity, TProperty>> expression, SortOrder sortOrder = SortOrder.Asc)
        //{
        //    if (expression == null) throw new ArgumentNullException(nameof(expression));

        //    OrderByConstraints.Add(
        //        new OrderByConstraint<TEntity, TProperty>(expression, sortOrder));

        //    return this;
        //}

        public FetchRequestMessage<TEntity, TSpecification> Include<TProperty>(Expression<Func<TEntity, TProperty>> expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            IncludeConstraints.Add(expression);

            return this;
        }


        public FetchRequestMessage<TEntity, TSpecification> Execute()
        {
            throw new NotImplementedException();
        }

        ISpecificationQuery<TEntity, TSpecification, FetchRequestMessage<TEntity, TSpecification>> ISpecificationQuery<TEntity, TSpecification, FetchRequestMessage<TEntity, TSpecification>>.Where(TSpecification specification)
        {
            return Where(specification);
        }
    }
}