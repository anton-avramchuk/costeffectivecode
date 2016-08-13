namespace CosteffectiveCode.Metadata
{
    public interface IOperation<in TInput, out TResult>
    {
        string Name { get; }

        string Description { get; }

        TResult Execute(TInput input);
    }
}
