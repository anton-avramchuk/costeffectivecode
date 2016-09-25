using System;
using System.Linq;
using System.Reflection;
using AutoMapper;
using CostEffectiveCode.Ddd.Entities;
using CostEffectiveCode.Ddd.Specifications.UnitOfWork;
using CostEffectiveCode.Ddd.UnitOfWork;

namespace CostEffectiveCode.AutoMapper
{
    public class DtoEntityTypeConverter<TDto, TEntity> : ITypeConverter<TDto, TEntity>
            where TEntity : class, IHasId, new()     
    {
        private readonly ILinqProvider _linqProvider;

        private readonly IUnitOfWork _unitOfWork;

        public DtoEntityTypeConverter(ILinqProvider linqProvider, IUnitOfWork unitOfWork)
        {
            _linqProvider = linqProvider;
            _unitOfWork = unitOfWork;
        }

        public TEntity Convert(TDto source, TEntity destination, ResolutionContext context)
        {
            var sourceId = (source as IHasId)?.Id;

            var dest = destination ?? (sourceId != null
                ? _linqProvider.GetQueryable<TEntity>().SingleOrDefault(x => x.Id.Equals(sourceId))
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
                if (key.EndsWith("ID")
                    && typeof(IHasId).GetTypeInfo().IsAssignableFrom(propertyInfo.PropertyType))
                {
                    propertyInfo.SetValue(dest, _unitOfWork.Find(propertyInfo.PropertyType, sp[key].GetValue(source)));
                }
                else
                {
                    if (propertyInfo.PropertyType != sp[key].PropertyType)
                    {
                        throw new InvalidOperationException($"Can't map Property {propertyInfo.Name} because of type mismatch:" +
                                                            $"{sp[key].PropertyType.Name} -> {propertyInfo.PropertyType.Name}");    
                    }

                    propertyInfo.SetValue(dest, sp[key].GetValue(source));
                }
            }

            return dest;
        }
    }
}
