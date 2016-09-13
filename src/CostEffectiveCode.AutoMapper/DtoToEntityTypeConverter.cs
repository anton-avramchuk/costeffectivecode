using System.Linq;
using System.Reflection;
using AutoMapper;
using CostEffectiveCode.Ddd.Entities;
using CostEffectiveCode.Ddd.Specifications.UnitOfWork;

namespace CostEffectiveCode.AutoMapper
{
    public class DtoEntityTypeConverter<TDto, TEntity> : ITypeConverter<TDto, TEntity>
            where TEntity : class, IEntity, new()     
    {
        private readonly ILinqProvider _linqProvider;

        public DtoEntityTypeConverter(ILinqProvider linqProvider)
        {
            _linqProvider = linqProvider;
        }

        public TEntity Convert(TDto source, TEntity destination, ResolutionContext context)
        {
            var sourceId = (source as IEntity)?.Id;

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
                var key = typeof(IEntity).GetTypeInfo().IsAssignableFrom(propertyInfo.PropertyType)
                    ? propertyInfo.Name.ToUpper() + "ID"
                    : propertyInfo.Name.ToUpper();

                if (sp.ContainsKey(key))
                {
                    // маппим один к одному примитивы, связанные сущности тащим из контекста
                    propertyInfo.SetValue(dest, key.EndsWith("ID")
                        && typeof(IEntity).GetTypeInfo().IsAssignableFrom(propertyInfo.PropertyType)
#warning FIX IT!
                            ? null
                            : sp[key].GetValue(source));
                }
            }

            return dest;
        }
    }
}
