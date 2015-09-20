using JetBrains.Annotations;

namespace CostEffectiveCode.Reflection
{
    [PublicAPI]
    public interface IGetter<in TObject, out TProperty>
    {
        TProperty Get(TObject @object);
    }

    [PublicAPI]
    public interface IGetter
    {
        object Get(object @object);
    }
}
