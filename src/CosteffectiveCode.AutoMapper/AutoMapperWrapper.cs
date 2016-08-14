using System;
using System.Collections.Concurrent;
using CosteffectiveCode.Common;
using AM = AutoMapper;
namespace CosteffectiveCode.AutoMapper
{
    public class AutoMapperWrapper : IMapper
    {
        public TReturn Map<TReturn>(object src) => AM.Mapper.Map<TReturn>(src);

        public TReturn Map<TReturn>(object src, TReturn dest) => AM.Mapper.Map(src, dest);

        internal static ConcurrentDictionary<Type, ConcurrentDictionary<Type, Action<AM.IMapperConfigurationExpression>>> TypeMap
            = new ConcurrentDictionary<Type, ConcurrentDictionary<Type, Action<AM.IMapperConfigurationExpression>>>();

        public static void Init()
        {
            AM.Mapper.Initialize(InitMappings);
            AM.Mapper.AssertConfigurationIsValid();
        }

        private static void InitMappings(AM.IMapperConfigurationExpression c)
        {
            foreach (var sourceType in TypeMap)
            {
                foreach (var destType in sourceType.Value)
                {
                    destType.Value.Invoke(c);
                }                  
            }

            c.CreateMissingTypeMaps = true;
        }
    }
}
