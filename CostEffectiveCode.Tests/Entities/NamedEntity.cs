using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CostEffectiveCode.Domain.Ddd.Entities;
using JetBrains.Annotations;

namespace CostEffectiveCode.EntityFramework.Tests.Entities
{
    public abstract class NamedEntity : EntityBase<long>
    {
        public const int NameMaxLength = 255;

        [Required, Index("IX_Name", 1, IsUnique = true), StringLength(NameMaxLength)]
        public string Name { get; set; }

        // EF Only
        internal NamedEntity()
        {
            
        }

        protected NamedEntity([NotNull] string name)
        {
            if (name == null) throw new ArgumentNullException("name");
            Name = name;         
        }
    }
}
