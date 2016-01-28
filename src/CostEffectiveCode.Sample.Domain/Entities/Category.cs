using System.Collections.Generic;
using CostEffectiveCode.Domain;
using CostEffectiveCode.Domain.Ddd;
using JetBrains.Annotations;

namespace CostEffectiveCode.Sample.Domain.Entities
{
    public class Category : NamedEntityBase
    {
        private List<Product> _lazyProducts = new List<Product>();

        private AggregationRootCollection<Category, Product> _products;
        
        // EF Mapping
        internal virtual ICollection<Product> LazyProducts
        {
            get { return _lazyProducts; }
            set { _lazyProducts = new List<Product>(value); }
        }


        public AggregationRootCollection<Category, Product> Products =>
            _products ?? (_products = new AggregationRootCollection<Category, Product>(this, LazyProducts));


        internal Category()
        {
            
        }

        public Category([NotNull] string name):base(name)
        {
            this.ValidateSelf();
        }
    }
}
