using System;
using System.Data.Entity;
using AutoMapper;
using CostEffectiveCode.Common;
using CostEffectiveCode.Domain.Ddd.Entities;
using CostEffectiveCode.Domain.Ddd.UnitOfWork;
using JetBrains.Annotations;

namespace CostEffectiveCode.AutoMapper
{
    public class AutoMapperWrapper : IMapper
    {
        public TReturn Map<TReturn>(object obj)
        {
            if (obj == null)
            {
                if (typeof(TReturn).IsAssignableFrom(typeof(ValueType)))
                {
                    throw new ArgumentException(nameof(Map));
                }

                return default(TReturn);
            }

            // TODO: check performance
            var resultType = typeof(TReturn);
            if (resultType.IsInstanceOfType(obj))
            {
                return (TReturn)obj;
            }

            return Mapper.Map<TReturn>(obj);
        }

        public TReturn Map<TReturn>(object obj, [NotNull] TReturn ret)
        {
            if (ret == null) throw new ArgumentNullException(nameof(ret));
            return Mapper.Map(obj, ret);
        }

        public object CreateMap<TFrom, TTo>()
        {
            return Mapper.CreateMap<TFrom, TTo>();
        }

        #region ViewModel to Entity and otherwise conventional mapping - TODO: refactor
        /*
        private readonly IScope<IDataContext> _dataContextScope;

        public AutoMapperWrapper([NotNull] IScope<IDataContext> dataContextScope)
        {
            if (dataContextScope == null) throw new ArgumentNullException(nameof(dataContextScope));

            _dataContextScope = dataContextScope;
        }

        public object CreateMapToEntity<TFrom, TTo>()
            where TTo : class, IEntity
        {
            // security-check: from-object should not be an entity! please prefer viewmodel!
            if (typeof(TFrom).IsAssignableFrom(typeof(IEntity)))
            {
                return CreateMap<TFrom, TTo>();
            }

            return Mapper.CreateMap<TFrom, TTo>()
                .ConstructUsing(LoadEntity<TFrom, TTo>);
        }

        private TTo LoadEntity<TFrom, TTo>(TFrom fromObj)
            where TTo : class, IEntity
        {
            var idProperty = fromObj
                .GetType()
                .GetProperty("Id");

            if (idProperty == null)
            {
                throw new NotSupportedException("You must provide Id field in ViewModel to support awesome automapping");
            }

            var idValue = idProperty.GetValue(fromObj);

            var idPropertyType = idProperty.PropertyType;

            var defaultKeyValue = idPropertyType.IsValueType ?
                Activator.CreateInstance(idPropertyType) :
                null;

            // Load Entity if ViewModel is not new
            // TODO: ViewModels should implement IEntity as well to work with Id without reflection

            if (idValue.Equals(defaultKeyValue))
            {
                return (TTo)Activator.CreateInstance(typeof(TTo));
            }

#warning Quick dirty hack, because of lack of time to do it right way ;(
            var dbSet = (IDbSet<TTo>)_dataContextScope
                    .Instance
                    .Query<TTo>();

            return dbSet.Find(idValue);
        }
        */
        #endregion
    }
}
