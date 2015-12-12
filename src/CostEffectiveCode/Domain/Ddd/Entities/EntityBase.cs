using System.ComponentModel.DataAnnotations;

namespace CostEffectiveCode.Domain.Ddd.Entities
{
    public interface IEntityBase<out T> : IEntity
    {
        T Id { get; }
    }

    public abstract class EntityBase<T> : IEntityBase<T>
    {
        /// <summary>
        /// To force derived types define proper constructors and keep invariant
        /// </summary>
        protected EntityBase()
        {
        }

        [Key, Required]
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
    }
}
