using System.Web.Http.Dependencies;
using CostEffectiveCode.Common;

namespace CostEffectiveCode.WebApi2.DI
{
    public class DependencyResolverDiContainer : IDiContainer
    {
        private readonly IDependencyResolver _dependencyResolver;

        public DependencyResolverDiContainer(IDependencyResolver dependencyResolver)
        {
            _dependencyResolver = dependencyResolver;
        }

        public T Resolve<T>()
        {
            return (T)_dependencyResolver.GetService(typeof(T));
        }
    }
}