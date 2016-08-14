namespace CosteffectiveCode.Domain.Ddd.Entities
{
    public interface IEntityBase<out T> : IEntity
    {
        new T Id { get; }       
    }

    public abstract class EntityBase<T> : IEntityBase<T>
    {
        public T Id { get; set; }

        public string GetId()
        {
                return Id != null
                    ? Id.ToString()
                    : string.Empty;

        }

        public bool IsNew()
        {
            return !string.IsNullOrEmpty(GetId());
        }

        object IEntity.Id => Id;
    }
}
