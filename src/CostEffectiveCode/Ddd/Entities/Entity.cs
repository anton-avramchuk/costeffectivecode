namespace CostEffectiveCode.Ddd.Entities
{
    public abstract class Entity<T> : IEntity<T>
    {
        public T Id { get; set; }

        public bool IsNew()
        {
            return !Id?.Equals(default(T)) ?? true;
        }

        object IEntity.Id => Id;
    }
}
