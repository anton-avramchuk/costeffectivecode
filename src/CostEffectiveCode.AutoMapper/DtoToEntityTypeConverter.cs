using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using CostEffectiveCode.Ddd;
using CostEffectiveCode.Ddd.Entities;

namespace CostEffectiveCode.AutoMapper
{
    public class DtoEntityTypeConverter<TKey, TDto, TEntity> : ITypeConverter<TDto, TEntity>
            where TEntity : class, IHasId<TKey>, new()     
    {
        protected readonly IUnitOfWork UnitOfWork;

        public DtoEntityTypeConverter(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public virtual TEntity Convert(TDto source, TEntity destination, ResolutionContext context)
        {
            var sourceId = (source as IHasId)?.Id;

            var dest = destination ?? (sourceId != null
                ? UnitOfWork.Find<TEntity>(sourceId) ?? new TEntity()
                : new TEntity());

            // Да, reflection, да медленно и может привести к ошибкам в рантайме.
            // Можете написать Expression Trees, скомпилировать и закешировать для производительности
            // И анализатор для проверки корректности Dto на этапе компиляции
            var sp = typeof(TDto)
                .GetTypeInfo()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(x => x.CanRead && x.CanWrite)
                .ToDictionary(x => x.Name.ToUpper(), x => x);

            var dp = typeof(TEntity)
                .GetTypeInfo()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(x => x.CanRead && x.CanWrite)
                .ToArray();

            // проходимся по всем свойствам целевого объекта
            foreach (var propertyInfo in dp)
            {
                var key = typeof(IHasId).GetTypeInfo().IsAssignableFrom(propertyInfo.PropertyType)
                    ? propertyInfo.Name.ToUpper() + "ID"
                    : propertyInfo.Name.ToUpper();

                if (!sp.ContainsKey(key)) continue;

                // маппим один к одному примитивы, связанные сущности тащим из контекста
                if (key.EndsWith("ID", StringComparison.CurrentCultureIgnoreCase)
                    && typeof(IHasId).GetTypeInfo().IsAssignableFrom(propertyInfo.PropertyType))
                {
                    propertyInfo.SetValue(dest, UnitOfWork.Find(propertyInfo.PropertyType, sp[key].GetValue(source)));
                }
                else
                {
                    if (propertyInfo.PropertyType != sp[key].PropertyType)
                    {
                        // маппим коллекции
                        var et = IsEntityGenericColections(sp[key].PropertyType, propertyInfo.PropertyType);
                        if (et != null)
                        {
                            var collection = propertyInfo.GetValue(dest);
                            var add = collection.GetType().GetTypeInfo().GetMethod("Add");
                            if (add != null)
                            {
                                var ids = (IEnumerable)sp[key].GetValue(source);
                                if (ids != null)
                                {
                                    foreach (var id in ids)
                                    {
                                        add.Invoke(collection, new object[] { UnitOfWork.Find(et, id) });
                                    }
                                }
                            }
                            else
                            {
                                throw new InvalidOperationException($"Can't map Property {propertyInfo.Name} because of type mismatch:" +
                                                                    $"{sp[key].PropertyType.Name} -> {propertyInfo.PropertyType.Name}");
                            }
                        }

                    }
                    else
                    {
                        propertyInfo.SetValue(dest, sp[key].GetValue(source));
                    }
                }
            }

            return dest;
        }

        private static Type IsEntityGenericColections(Type src, Type dest)
        {
            if (!dest.GetTypeInfo().IsGenericType) return null;
            if (dest.GetTypeInfo().GetGenericArguments().Length > 1) return null;

            if (!typeof(IEnumerable).GetTypeInfo().IsAssignableFrom(src) ||
                typeof(ICollection<>) != dest.GetGenericTypeDefinition()
                && !dest.GetTypeInfo().GetInterfaces().Any(x => x.GetTypeInfo().IsGenericType
                && x.GetTypeInfo().GetGenericTypeDefinition() == typeof(ICollection<>)))
            {
                return null;
            }

            return dest.GetTypeInfo().GetGenericArguments().First();
        }
    }
}
