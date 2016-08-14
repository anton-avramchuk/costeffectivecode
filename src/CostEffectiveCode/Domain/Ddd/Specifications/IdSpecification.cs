using System.Collections.Generic;
using CosteffectiveCode.Domain.Ddd.Entities;
using JetBrains.Annotations;

namespace CosteffectiveCode.Domain.Ddd.Specifications
{
    [PublicAPI]
    public class IdSpecification<T> : ExpressionSpecification<T>
        where T : class, IEntityBase<long>
    {
        public long Id { get; private set; }

        public IdSpecification(long id)
            : base(x => x.Id == id)
        {
            Id = id;
        }

        public KeyValuePair<string, object> KeyValue => new KeyValuePair<string, object>("Id", Id);
    }
}
