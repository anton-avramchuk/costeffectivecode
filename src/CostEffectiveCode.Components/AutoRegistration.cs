using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CostEffectiveCode.Components.Cqrs;
using CostEffectiveCode.Cqrs;
using CostEffectiveCode.Cqrs.Queries;
using CostEffectiveCode.Ddd.Specifications;
using CostEffectiveCode.Extensions;
using JetBrains.Annotations;

namespace CostEffectiveCode.Components
{
    public static class AutoRegistration
    {
        [CanBeNull]
        private static Type GetFallBack(Type type)
        {
            var ti = type.GetTypeInfo();
            if (!ti.IsInterface || !ti.IsGenericType) return null;
            var generic = type.GetGenericTypeDefinition();
            var genericArgs = ti.GetGenericArguments();

            if (generic == typeof(IQuery<,>))
            {
                var firstArgInterfaces = genericArgs[0].GetTypeInfo().GetInterfaces();
                var secondArgInterface = genericArgs[1];
                var paging = firstArgInterfaces.FirstOrDefault(i => ImplementsOpenGeneric(i, typeof(IPaging<,>)));
                if (paging != null && ImplementsOpenGeneric(secondArgInterface, typeof(IPagedEnumerable<>)))
                {
                    var dtoType = genericArgs[1].GetTypeInfo().GetGenericArguments()[0];
                    var entityType = dtoType.GetTypeInfo().GetCustomAttribute<DtoForAttribute>()?.EntityType;
                    if (entityType == null) return null;
                    var sortKey = paging.GetTypeInfo().GetGenericArguments()[1];
                    return typeof(PagedQuery<,,,>).MakeGenericType(genericArgs[0], entityType, dtoType, sortKey);
                }
            }

            return null;
        }

        private static bool ImplementsOpenGeneric(Type i, Type checkType)
        {
            return i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == checkType;
        }

        public static Dictionary<Type, Type> GetComponentMap(
            Assembly dependentAssembly,
            Func<Type, bool> dependentTypeSpec,
            Assembly sourceAssembly,
            Func<Type, bool> sourceTypeSpec)
        {
            var res = new Dictionary<Type, Type>();
            var types = sourceAssembly
                .GetTypes()
                .ToArray();

            var dependencies = dependentAssembly
                .GetTypes()
                .Where(dependentTypeSpec.Invoke)
                .Select(x => x
                    .GetTypeInfo()
                    .GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                    .OrderByDescending(y => y.GetParameters().Length)
                    .FirstOrDefault())
                .Where(x => x != null)
                .SelectMany(x => x.GetParameters()
                    .Where(y => sourceTypeSpec.Invoke(y.ParameterType)))
                .Distinct()
                .ToArray();

            foreach (var dep in dependencies)
            {
                var attr = dep.GetCustomAttribute<ImplementationAttribute>();
                if (attr != null)
                {
                    if (!dep.ParameterType.GetTypeInfo().IsAssignableFrom(attr.Implementation))
                    {
                        throw new InvalidOperationException(
                            $"{attr.Implementation} can't implement {dep.ParameterType} in " +
                            $"{dep.Member.DeclaringType?.Name}:{dep.Name} constructor parameter.");
                    }

                    res.Add(dep.ParameterType, attr.Implementation);
                    continue;
                }

                var implementations = types
                    .Where(x => dep.ParameterType.GetTypeInfo().IsAssignableFrom(x))
                    .ToArray();

                if (implementations.Length > 1)
                {
                    var aggr = implementations.Select(x => x.Name).Aggregate((c, n) => $"{c},{n}");
                    throw new InvalidOperationException($"{aggr} implementations found for {dep.Name}. " +
                                                        $"You must have only one implementation per assembly");
                }

                var implementation = implementations.FirstOrDefault() ?? GetFallBack(dep.ParameterType);

                if (implementation != null)
                {
                    res.Add(dep.ParameterType, implementation);
                }
                else
                {
                    throw new InvalidOperationException($"Can't find implementation for type {dep.ParameterType} in  " +
                                                        $"{dep.Member.DeclaringType?.Name}:{dep.Name} constructor parameter. " +
                                                        $"Use Implementation Attribute to configure implementation explicitly. By the way don't you forget to use [DtoFor] attribute?");
                }
            }

            return res;
        }
    }
}
