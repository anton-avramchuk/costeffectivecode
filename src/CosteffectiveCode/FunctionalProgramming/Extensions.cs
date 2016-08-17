using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using CosteffectiveCode.Ddd.Entities;
using CosteffectiveCode.Ddd.Specifications;

namespace CosteffectiveCode.FunctionalProgramming
{
    public static class Extensions
    {
        public static Type[] GetMorphismTypes(this Assembly assembly, ISpecification<Type> spec) =>
            assembly
                .ExportedTypes
                .Where(x =>
                {
                    var t = x.GetTypeInfo();
                    return t.IsClass && !t.IsAbstract && t.ImplementedInterfaces.Any(i =>
                    {
                        var ti = i.GetTypeInfo();
                        return ti.IsGenericType && ti.GetGenericTypeDefinition() == typeof(IMorphism<,>);
                    }) && spec.IsSatisfiedBy(x);
                })
                .ToArray();

        public static void DefineList<TEntity, TDto, TMorphism>(this Category category)
            where TEntity: IEntity
            where TMorphism: IMorphism<ExpressionSpecification<TEntity>, TDto[]>
        {
            category.Define<ExpressionSpecification<TEntity>, TDto[], TMorphism>();
        }

        public static bool ImplementsOpenGeneric(this Type type, Type openGenericType)
        {
            var ti = type.GetTypeInfo();
            return ti.IsGenericType && ti.GetGenericTypeDefinition() == openGenericType;
        }

        public static TResult Execute<TEntity, TResult>(
            this IMorphism<ExpressionSpecification<TEntity>, TResult> morphism, Expression<Func<TEntity, bool>> expression)
        {
            return morphism.Execute(new ExpressionSpecification<TEntity>(expression));
        }
    }
}
