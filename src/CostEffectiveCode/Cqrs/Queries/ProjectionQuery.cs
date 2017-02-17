using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CostEffectiveCode.Common;
using CostEffectiveCode.Ddd;
using CostEffectiveCode.Ddd.Entities;
using CostEffectiveCode.Ddd.Pagination;
using CostEffectiveCode.Ddd.Specifications;
using CostEffectiveCode.Extensions;
using JetBrains.Annotations;

namespace CostEffectiveCode.Cqrs.Queries
{
    public class ProjectionQuery<TSpecification, TSource, TDest>
        : IQuery<TSpecification, IEnumerable<TDest>>
        , IQuery<TSpecification, int>
        where TSource : class, IHasId
        where TDest : class
    {
        protected readonly ILinqProvider LinqProvider;
        protected readonly IProjector Projector;

        private static readonly Type[] SpecTypes = {
            typeof(ILinqSorting<TSource>),
            typeof(ILinqSorting<TDest>),
            typeof(ILinqSpecification<TSource>),
            typeof(ILinqSpecification<TDest>),
            typeof(Expression<Func<TSource,bool>>),
            typeof(Expression<Func<TDest,bool>>),
            typeof(ExpressionSpecification<TSource>),
            typeof(ExpressionSpecification<TDest>)
        };

        private static string ErrorMessage => SpecTypes.Select(x => x.ToString()).Aggregate((c, n) => $"{c}\n{n}");

        public ProjectionQuery([NotNull] ILinqProvider linqProvider, [NotNull] IProjector projector)
        {
            if (linqProvider == null) throw new ArgumentNullException(nameof(linqProvider));
            if (projector == null) throw new ArgumentNullException(nameof(projector));

            LinqProvider = linqProvider;
            Projector = projector;
        }

        protected virtual IQueryable<TDest> GetQueryable(TSpecification spec)
        {
            return LinqProvider
                .Query<TSource>()
                .MaybeWhere(spec)
                .MaybeSort(spec)
                .Project<TDest>(Projector)
                .MaybeWhere(spec)
                .MaybeSort(spec);
        } 

        public virtual IEnumerable<TDest> Ask(TSpecification specification) => GetQueryable(specification).ToArray();

        int IQuery<TSpecification, int>.Ask(TSpecification specification) => GetQueryable(specification).Count();
    }
}
