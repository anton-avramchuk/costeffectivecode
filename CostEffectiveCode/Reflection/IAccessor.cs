namespace CostEffectiveCode.Reflection
{
    public interface IAccessor<in TObject, TProperty> : ISetter<TObject, TProperty>, IGetter<TObject, TProperty>,
        ISetter, IGetter
    {
    }

}
