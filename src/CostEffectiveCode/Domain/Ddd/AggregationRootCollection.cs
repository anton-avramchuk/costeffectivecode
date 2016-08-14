using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CosteffectiveCode.Domain.Ddd.Entities;
using JetBrains.Annotations;

namespace CosteffectiveCode.Domain.Ddd
{
    [PublicAPI]
    public class AggregationRootCollection<TAggregationRoot, TChild> : ICollection<TChild>
        where TChild: class, IEntity
        where TAggregationRoot: class, IEntity
    {
        #region Fields

        private readonly TAggregationRoot _root;
        private readonly ICollection<TChild> _storage;

        #endregion

        #region Static Reflection Optimization

        // ReSharper disable once StaticMemberInGenericType
        private static readonly Dictionary<string, PropertyInfo> AggreagationRootProperties =
            new Dictionary<string, PropertyInfo>();

        private static PropertyInfo GetAggregationRootProperty()
        {
            var key = $"{typeof (TAggregationRoot)}{typeof (TChild)}";
            if (!AggreagationRootProperties.ContainsKey(key))
            {
                AggreagationRootProperties[key] = typeof(TChild)
                    .GetRuntimeProperties()
                    .Single(p => p.PropertyType == typeof(TAggregationRoot) && p.CanWrite);
            }

            return AggreagationRootProperties[key];
        }

        private static void SetAggregationRoot(TChild i1, TAggregationRoot r1)
        {
            try
            {
                GetAggregationRootProperty().SetValue(i1, r1, null);
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException ?? e;
            }
        }

        #endregion

        #region Constructors
        
        public AggregationRootCollection(
            [NotNull] TAggregationRoot root,
            [NotNull] ICollection<TChild> storage )
        {
            if (root == null) throw new ArgumentNullException(nameof(root));
            if (storage == null) throw new ArgumentNullException(nameof(storage));

            _root = root;
            _storage = storage.IsReadOnly
                ? new List<TChild>(storage) 
                : storage;
        }

        #endregion

        #region ICollection implementation

        public IEnumerator<TChild> GetEnumerator()
        {
            return _storage.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(TChild item)
        {
            _storage.Add(item);
            SetAggregationRoot(item, _root);
        }



        public void Clear()
        {
            foreach (var item in this)
            {
                SetAggregationRoot(item, null);
            }
            
            _storage.Clear();
        }

        public bool Contains(TChild item)
        {
            return _storage.Contains(item);
        }

        public void CopyTo(TChild[] array, int arrayIndex)
        {
            _storage.CopyTo(array, arrayIndex);
        }

        public bool Remove(TChild item)
        {
            var res = _storage.Remove(item);
            if (res)
            {
                SetAggregationRoot(item, null);
            }

            return res;
        }

        public int Count => _storage.Count;

        public bool IsReadOnly => false;

        #endregion
    }
}
