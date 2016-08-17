using CosteffectiveCode.Ddd.Entities;
using JetBrains.Annotations;

namespace CosteffectiveCode.Ddd.Specifications
{
    [PublicAPI]
    public class IdSpecification<TKey,T> : ExpressionSpecification<T>
        where T : IEntityBase<TKey>
    {
        public TKey Id { get; private set; }

        public IdSpecification(TKey id)
            : base(x => x.Id.Equals(id))
        {
            Id = id;
        }
    }
}
