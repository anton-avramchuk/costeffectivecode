using JetBrains.Annotations;

namespace CostEffectiveCode.Reflection
{
    [PublicAPI]
    public interface IAccessor<in TObject, TProperty>
        : ISetter<TObject, TProperty>, IGetter<TObject, TProperty>, ISetter, IGetter
    {
    }

}
