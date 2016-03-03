using CostEffectiveCode.Domain.Ddd.Entities;
using CostEffectiveCode.Domain.Ddd.Specifications;

namespace CostEffectiveCode.Akka.Messages
{
    public class FetchRequestMessage<TEntity> : FetchRequestMessage<TEntity, IExpressionSpecification<TEntity>>
        where TEntity : class, IEntity
    {
        public FetchRequestMessage(bool single, bool firstOrDefault) : base(single, firstOrDefault)
        {
        }

        public FetchRequestMessage(int page, int limit) : base(page, limit)
        {
        }

        public FetchRequestMessage(int limit) : base(limit)
        {
        }

        public FetchRequestMessage() : base()
        {
        }
    }
}
