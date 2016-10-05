using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;

namespace CostEffectiveCode.AutoMapper
{
    public class ConventionalProfile : Profile
    {
        public static IDictionary<Type, Type[]> TypeMap;

        public static void Scan(params Assembly[] assemblies)
        {
            TypeMap = assemblies
                .SelectMany(x => x.GetTypes())
                .Where(x => x.GetTypeInfo().GetCustomAttribute<ConventionalMapAttribute>() != null)
                .GroupBy(x => x.GetTypeInfo().GetCustomAttribute<ConventionalMapAttribute>().EntityType)
                .ToDictionary(k => k.Key, v => v.ToArray());
        }

        public ConventionalProfile()
        {
            if (TypeMap == null)
            {
                throw new InvalidOperationException("Use ConventionalProfile.Scan method first!");
            }

            foreach (var kv in TypeMap)
            {
                foreach (var v in kv.Value)
                {
                    var attr = v.GetTypeInfo().GetCustomAttribute<ConventionalMapAttribute>();
                    if (attr.Direction == MapDirection.EntityToDto || attr.Direction == MapDirection.Both)
                    {
                        CreateMap(kv.Key, v);
                    }
                    if (attr.Direction == MapDirection.DtoToEntity || attr.Direction == MapDirection.Both)
                    {
                        CreateMap(v, kv.Key).ConvertUsing(typeof(DtoEntityTypeConverter<,,>)
                            .MakeGenericType(kv.Key.GetTypeInfo().GetProperty("Id").GetType(), v, kv.Key));
                    }
                }
            }
        }
    }
}
