using System;

namespace CostEffectiveCode.Cqrs
{
    public interface ITypeAssociation
    {
        Type EntityType { get; }
    }
}