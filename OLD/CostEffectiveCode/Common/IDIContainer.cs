namespace CostEffectiveCode.Common
{
    public interface IDiContainer
    {
        T Resolve<T>();
    }
}
