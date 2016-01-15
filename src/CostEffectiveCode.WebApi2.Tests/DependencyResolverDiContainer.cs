using CostEffectiveCode.Common;

namespace CostEffectiveCode.WebApi2.Tests
{
    public class DependencyResolverDiContainer : IDiContainer
    {
        public T Resolve<T>()
        {
            return (T)Startup.HttpConfiguration.DependencyResolver.GetService(typeof(T));
        }
    }
}