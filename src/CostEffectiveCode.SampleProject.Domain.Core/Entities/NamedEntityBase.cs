using System;
using System.ComponentModel.DataAnnotations;
using CostEffectiveCode.Domain.Ddd.Entities;
using JetBrains.Annotations;

namespace CostEffectiveCode.SampleProject.Domain.Shared.Entities
{
    public abstract class NamedEntityBase : EntityBase<long>
    {
        public const int NameMaxLength = 255;

        [Required] //, Index("IX_Name", 1, IsUnique = true), StringLength(NameMaxLength)]
        public string Name { get; set; }

        // EF Only
        internal NamedEntityBase()
        {
            
        }

        protected NamedEntityBase([NotNull] string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            Name = name;         
        }
    }
}
