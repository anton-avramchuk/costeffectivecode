namespace CostEffectiveCode.Reflection
{
    public interface IGetter<in TObject, out TProperty>
    {
        TProperty Get(TObject @object);
    }

    public interface IGetter
    {
        object Get(object @object);
    }
}
