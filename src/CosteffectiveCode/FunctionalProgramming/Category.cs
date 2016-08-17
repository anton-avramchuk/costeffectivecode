using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using CosteffectiveCode.Ddd.Entities;
using JetBrains.Annotations;

namespace CosteffectiveCode.FunctionalProgramming
{
    public class Category : IEntity
    {
        public string Id { get; }

        object IEntity.Id => Id;

        private readonly ConcurrentDictionary<Type, ConcurrentDictionary<Type, Type>> _typeMap
            = new ConcurrentDictionary<Type, ConcurrentDictionary<Type,Type>>();

        public Category([NotNull] string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));
            Id = id;
        }

        public void Define(Type leftObjectType, Type rihtObjectType, Type morphismType)
        {
            if (!morphismType.GetTypeInfo().ImplementedInterfaces.Any(x =>
            {
                var ti = x.GetTypeInfo();
                return ti.IsGenericType && ti.GetGenericTypeDefinition() == typeof(IMorphism<,>);
            }))
            {
                throw new ArgumentException("morpismType must implement IMorphism<,>", nameof(morphismType));
            }
            DoDefine(leftObjectType, rihtObjectType, morphismType);
        }

        private void DoDefine(Type leftObjectType, Type rihtObjectType, Type morphismType)
        {
            _typeMap
                .GetOrAdd(leftObjectType, x => new ConcurrentDictionary<Type, Type>())
                .AddOrUpdate(rihtObjectType, x => morphismType, (x, y) => morphismType);
        }

        public void Define<TLeft, TRight, TMorphism>()
            where TMorphism: IMorphism<TLeft, TRight>
        {
            DoDefine(typeof(TLeft), typeof(TRight), typeof(TMorphism));
        }

        public ReadOnlyDictionary<Type, ReadOnlyDictionary<Type, Type>> ObjectsAndMorphisms =>
            new ReadOnlyDictionary<Type, ReadOnlyDictionary<Type, Type>>(_typeMap.ToDictionary(x => x.Key,
                x => new ReadOnlyDictionary<Type, Type>(x.Value.ToDictionary(y => y.Key, y => y.Value))));
    }
}
