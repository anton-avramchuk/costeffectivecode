namespace CosteffectiveCode.Ddd.Entities
{
    public interface IEntityBase<out T> : IEntity
    {
        new T Id { get; }       
    }

    public abstract class EntityBase<T> : IEntityBase<T>
    {
        public T Id { get; set; }

        public bool IsNew()
        {
            return !Id?.Equals(default(T)) ?? true;
        }

        object IEntity.Id => Id;
    }
}
