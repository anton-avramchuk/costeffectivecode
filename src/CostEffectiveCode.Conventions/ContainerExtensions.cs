using System;
using System.Linq;
using System.Reflection;
using CostEffectiveCode.Cqrs.Commands;
using JetBrains.Annotations;
using SimpleInjector;

namespace CostEffectiveCode.Conventions
{
    public static class ContainerExtensions
    {
        public static void RegisterCommands(this Container container, [NotNull] Assembly assembly,
            [NotNull] Func<Type, bool> typeSpec, [NotNull] Func<Assembly, Type, Type> dtoSelector)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));
            if (typeSpec == null) throw new ArgumentNullException(nameof(typeSpec));
            if (dtoSelector == null) throw new ArgumentNullException(nameof(dtoSelector));

            foreach (var type in assembly
                .GetTypes()
                .Where(x => x.GetTypeInfo().ImplementedInterfaces.Any(IsCommandImplementation))
                .Where(typeSpec))
            {
                container.Register(type, typeof(ICommand<>).MakeGenericType(GetCommandInputOutput(type)));
            }
        }

        private static bool IsCommandImplementation(Type i)
        {
            var ti = i.GetTypeInfo();
            return ti.IsGenericType && ti.GetGenericTypeDefinition() == typeof(ICommand<,>);
        }

        private static Type[] GetCommandInputOutput(Type i)
            => i.GetTypeInfo()
                .ImplementedInterfaces
                .Single(IsCommandImplementation)
                .GetTypeInfo()
                .GetGenericArguments();
    }
}
