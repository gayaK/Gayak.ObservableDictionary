using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;

namespace Gayak.Collections
{
    [DebuggerDisplay("Count = {Count}")]
    public partial class CollectionBasedDictionary<TKey, TValue> :
        KeyedCollection<TKey, KeyValuePair<TKey, TValue>>,
        IDictionary<TKey, TValue>,
#if !NET40
        IReadOnlyDictionary<TKey, TValue>,
#endif
        IDictionary
    {
        public CollectionBasedDictionary() { }
        public CollectionBasedDictionary(IEqualityComparer<TKey> comparer) : base(comparer) { }
        public CollectionBasedDictionary(IDictionary<TKey, TValue> dictionary) : this(dictionary, null) { }
        public CollectionBasedDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer) : base(comparer)
        {
            foreach (var kvp in dictionary)
            {
                Add(kvp);
            }
        }

        public new TValue this[TKey key]
        {
            get => base[key].Value;
            set
            {
                var newKvp = new KeyValuePair<TKey, TValue>(key, value);
                if (TryGetValueInternal(key, out var oldKvp))
                {
                    SetItem(IndexOf(oldKvp), newKvp);
                }
                else
                {
                    Add(newKvp);
                }
            }
        }

        object IDictionary.this[object key]
        {
            get => this[(TKey)key];
            set => this[(TKey)key] = (TValue)value;
        }

        public KeyCollection Keys => _keys ?? (_keys = new KeyCollection(this));
        private KeyCollection _keys;
        ICollection<TKey> IDictionary<TKey, TValue>.Keys => this.Keys;
#if !NET40
        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => this.Keys;
#endif
        ICollection IDictionary.Keys => this.Keys;

        public ValueCollection Values => _values ?? (_values = new ValueCollection(this));
        private ValueCollection _values;
        ICollection<TValue> IDictionary<TKey, TValue>.Values => this.Values;
#if !NET40
        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => this.Values;
#endif
        ICollection IDictionary.Values => this.Values;

        public bool IsFixedSize => false;

        public bool IsReadOnly => false;

        protected override TKey GetKeyForItem(KeyValuePair<TKey, TValue> item) => item.Key;

        public void Add(TKey key, TValue value) => base.Add(new KeyValuePair<TKey, TValue>(key, value));
        void IDictionary.Add(object key, object value) => this.Add((TKey)key, (TValue)value);

        void IDictionary.Remove(object key) => this.Remove((TKey)key);

        public bool ContainsKey(TKey key) => Dictionary?.ContainsKey(key) ?? false;
        bool IDictionary.Contains(object key) => this.ContainsKey((TKey)key);

        public bool TryGetValue(TKey key, out TValue value)
        {
            var ret = TryGetValueInternal(key, out var kvp);
            value = kvp.Value;
            return ret;
        }

        protected bool TryGetValueInternal(TKey key, out KeyValuePair<TKey, TValue> value)
        {
            if (Dictionary == null)
            {
                value = default(KeyValuePair<TKey, TValue>);
                return false;
            }
            return Dictionary.TryGetValue(key, out value);
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return new DictionaryEnumerator(this.GetEnumerator());
        }
    }
}
