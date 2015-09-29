using System.Collections.Generic;

namespace CostEffectiveCode.Domain.Ddd.Specifications
{
    public interface IEqualsSpecification
    {
        KeyValuePair<string, object> KeyValue { get; } 
    }
}
