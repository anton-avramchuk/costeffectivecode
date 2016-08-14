namespace CosteffectiveCode.FunctionalProgramming
{
    public interface IMorphism<in TInput, out TResult>
    {
        TResult Execute(TInput input);
    }
}
