using System.Linq;
using AutoMapper.QueryableExtensions;
using CostEffectiveCode.Common;
using AM = AutoMapper;
using IMapper = CostEffectiveCode.Common.IMapper;

namespace CostEffectiveCode.AutoMapper
{
    public class InstanceBasedAutoMapperWrapper : IMapper, IProjector
    {
        public AM.IConfigurationProvider Configuration { get; private set; }
        public AM.IMapper Instance { get; private set; }

        public InstanceBasedAutoMapperWrapper(AM.IConfigurationProvider configuration, bool skipValidnessAssertation = false)
        {
            Configuration = configuration;

            if (!skipValidnessAssertation)
            {
                configuration.AssertConfigurationIsValid();
            }

            Instance = configuration.CreateMapper();
        }

        public TReturn Map<TReturn>(object src) => Instance.Map<TReturn>(src);

        public TReturn Map<TReturn>(object src, TReturn dest) => Instance.Map(src, dest);

        public IQueryable<TReturn> Project<TReturn>(IQueryable queryable)
            => queryable.ProjectTo<TReturn>(Configuration);
    }
}
