using System;
using System.ComponentModel.DataAnnotations;

namespace CostEffectiveCode.Ddd.Entities
{
    public abstract class HasIdBase<TKey> : IHasId<TKey>
        where TKey: IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        [Required]
        public virtual TKey Id { get; set; }

        object IHasId.Id => Id;
    }
}
