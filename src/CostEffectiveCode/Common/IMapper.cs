using JetBrains.Annotations;

namespace CosteffectiveCode.Common
{
    [PublicAPI]
    public interface IMapper
    {
        TReturn Map<TReturn>(object src);

        TReturn Map<TReturn>(object src, TReturn dest);
    }
}