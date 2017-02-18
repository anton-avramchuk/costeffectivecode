using System;

namespace CostEffectiveCode.Ddd.Entities
{
    public abstract class HasIdBase<TKey> : IHasId<TKey>
        where TKey: IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        public TKey Id { get; set; }

        public bool IsNew()
        {
            return Id == null || Id.Equals(default(TKey));
        }

        object IHasId.Id => Id;
    }
}
