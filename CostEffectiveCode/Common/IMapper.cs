using JetBrains.Annotations;

namespace CostEffectiveCode.Common
{
    [PublicAPI]
    public interface IMapper
    {
        TReturn Map<TReturn>(object obj);

        TReturn Map<TReturn>(object obj, TReturn ret);
    }
}