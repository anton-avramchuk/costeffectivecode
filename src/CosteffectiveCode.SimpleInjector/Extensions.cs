using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CosteffectiveCode.Ddd.Specifications;
using CosteffectiveCode.FunctionalProgramming;
using SI = SimpleInjector;
namespace CosteffectiveCode.SimpleInjector
{
    public static class Extensions
    {
        public static void RegisterCategories(this SI.Container container,
            SI.Lifestyle lifestyle,
            params Category[] categories)
        {
            foreach (var category in categories)
            {
                RegisterCategory(container, category, lifestyle);
            }

            container.RegisterCollection(categories);
        }

        private static void RegisterCategory(this SI.Container container, Category category, SI.Lifestyle lifestyle)
        {
            var map = category.ObjectsAndMorphisms
                .SelectMany(x => x.Value.Values);

            foreach (var morphism in map)
            {
                foreach (var implementation in morphism.GetTypeInfo().ImplementedInterfaces
                    .Where(x => x.ImplementsOpenGeneric(typeof(IMorphism<,>))))
                {
                    var genericArgs = implementation.GetTypeInfo().GenericTypeArguments;

                    container.Register(
                       typeof(IMorphism<,>).MakeGenericType(genericArgs[0], genericArgs[1]),
                       morphism,
                       lifestyle);
                }
            }
        }

        public static IMorphism<ExpressionSpecification<TEntity>, TDto[]> GetSpecMorphism<TEntity, TDto>(
            this SI.Container container) =>
                container.GetInstance<IMorphism<ExpressionSpecification<TEntity>, TDto[]>>();
    }
}
