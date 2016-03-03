namespace CostEffectiveCode.Common.Scope
{
    public class DiContainerScope<T> : IScope<T>
    {
        private readonly IDiContainer _container;

        public DiContainerScope(IDiContainer container)
        {
            _container = container;
        }

        public T Instance => _container.Resolve<T>();
    }
}