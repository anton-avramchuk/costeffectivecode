using JetBrains.Annotations;

namespace CostEffectiveCode.Reflection
{
    [PublicAPI]
    public interface ISetter<in TObject, in TProperty>
    {
        void Set(TObject @object, TProperty value);
    }

    [PublicAPI]
    public interface ISetter
    {
        void Set(object @object, object value);
    }
}
