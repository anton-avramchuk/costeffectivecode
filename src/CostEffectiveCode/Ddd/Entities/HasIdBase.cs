namespace CostEffectiveCode.Ddd.Entities
{
    public abstract class HasIdBase<T> : IHasId<T>
    {
        public T Id { get; set; }

        public bool IsNew()
        {
            return !Id?.Equals(default(T)) ?? true;
        }

        object IHasId.Id => Id;
    }
}
