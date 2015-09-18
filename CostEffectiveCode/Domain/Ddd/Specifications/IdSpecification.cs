using System.Collections.Generic;
using CostEffectiveCode.Domain.Ddd.Entities;
using JetBrains.Annotations;

namespace CostEffectiveCode.Domain.Ddd.Specifications
{
    [PublicAPI]
    public class IdSpecification<T> : ExpressionSpecification<T>, IEqualsSpecification
        where T : class, IEntityBase<long>
    {
        public long Id { get; private set; }

        public IdSpecification(long id)
            : base(x => x.Id == id)
        {
            Id = id;
        }

        public KeyValuePair<string, object> KeyValue
        {
            get
            {
                return new KeyValuePair<string, object>("Id", Id);
            }
        }
    }
}
