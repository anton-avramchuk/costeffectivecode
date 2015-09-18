namespace CostEffectiveCode.Domain.Ddd
{
    public interface IValidator<in T>
    {
        bool Validate(T obj);
    }
}
