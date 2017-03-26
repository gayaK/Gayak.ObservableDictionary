using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace Gayak.Collections
{
    [DebuggerDisplay("Count = {Count}")]
    public class ReadonlyObservableDictionary<TKey, TValue> :
        ReadOnlyCollection<KeyValuePair<TKey, TValue>>,
        IDictionary<TKey, TValue>,
#if !NET40
        IReadOnlyDictionary<TKey, TValue>,
#endif
        IDictionary,
        INotifyCollectionChanged,
        INotifyPropertyChanged
    {
        public ReadonlyObservableDictionary(ObservableDictionary<TKey, TValue> dic) : base(dic)
        {
            Dictionary = dic ?? throw new ArgumentNullException(nameof(dic));

            ((INotifyPropertyChanged)Dictionary).PropertyChanged += (_, e) => OnPropertyChanged(e);
            Dictionary.CollectionChanged += (_, e) => OnCollectionChanged(e);
        }

        protected virtual event NotifyCollectionChangedEventHandler CollectionChanged = (_, __) => { };
        event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged
        {
            add { CollectionChanged += value; }
            remove { CollectionChanged -= value; }
        }

        protected virtual event PropertyChangedEventHandler PropertyChanged;
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add { PropertyChanged += value; }
            remove { PropertyChanged -= value; }
        }

        protected ObservableDictionary<TKey, TValue> Dictionary { get; private set; }

        public TValue this[TKey key]
        {
            get => Dictionary[key];
            set => throw new NotSupportedException();
        }

        object IDictionary.this[object key]
        {
            get => this[(TKey)key];
            set => throw new NotSupportedException();
        }

        public CollectionBasedDictionary<TKey, TValue>.KeyCollection Keys => Dictionary.Keys;
        ICollection<TKey> IDictionary<TKey, TValue>.Keys => this.Keys;
#if !NET40
        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => this.Keys;
#endif
        ICollection IDictionary.Keys => this.Keys;

        public CollectionBasedDictionary<TKey, TValue>.ValueCollection Values => Dictionary.Values;
        ICollection<TValue> IDictionary<TKey, TValue>.Values => this.Values;
#if !NET40
        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => this.Values;
#endif
        ICollection IDictionary.Values => this.Values;

        public bool IsFixedSize => false;

        public bool IsReadOnly => true;

        public void Add(TKey key, TValue value) => throw new NotSupportedException();
        void IDictionary.Add(object key, object value) => throw new NotSupportedException();

        public bool Remove(TKey key) => throw new NotSupportedException();
        void IDictionary.Remove(object key) => throw new NotSupportedException();

        public bool ContainsKey(TKey key) => Dictionary.ContainsKey(key);
        bool IDictionary.Contains(object key) => this.ContainsKey((TKey)key);

        public void Clear() => throw new NotSupportedException();

        public bool TryGetValue(TKey key, out TValue value) => Dictionary.TryGetValue(key, out value);

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return ((IDictionary)Dictionary).GetEnumerator();
        }

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs args) => CollectionChanged(this, args);
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args) => PropertyChanged(this, args);
    }
}
