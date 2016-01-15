using Autofac;
using CostEffectiveCode.Common;

namespace CostEffectiveCode.Autofac
{
    public class AutofacDiContainer : IDiContainer
    {
        private readonly IContainer _container;
        public AutofacDiContainer(IContainer container)
        {
            _container = container;
        }

        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }
    }
}