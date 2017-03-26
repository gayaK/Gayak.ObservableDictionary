using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Gayak.Collections
{
    [DebuggerDisplay("Count = {Count}")]
    public class ObservableDictionary<TKey, TValue> :
        CollectionBasedDictionary<TKey, TValue>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        public ObservableDictionary() { }
        public ObservableDictionary(IEqualityComparer<TKey> comparer) : base(comparer) { }
        public ObservableDictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary) { }
        public ObservableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer) : base(dictionary, comparer) { }

        private static readonly string _indexerName = "Item[]";

        public event NotifyCollectionChangedEventHandler CollectionChanged = (_, __) => { };

        protected virtual event PropertyChangedEventHandler PropertyChanged = (_, __) => { };
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add => this.PropertyChanged += value;
            remove => this.PropertyChanged -= value;
        }

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged(this, e);
        }

#if NET40
        protected virtual void OnPropertyChanged(string name)
#else
        protected virtual void OnPropertyChanged([CallerMemberName]string name = "")
#endif
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        private void OnCollectionInserted(KeyValuePair<TKey, TValue> item, int index)
        {
            OnPropertyChanged(nameof(Count));
            OnPropertyChanged(_indexerName);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
        }

        private void OnCollectionRemoved(KeyValuePair<TKey, TValue> item, int index)
        {
            OnPropertyChanged(nameof(Count));
            OnPropertyChanged(_indexerName);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
        }

        private void OnCollectionSet(KeyValuePair<TKey, TValue> newItem, KeyValuePair<TKey, TValue> oldItem, int index)
        {
            OnPropertyChanged(_indexerName);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItem, oldItem, index));
        }

        private void OnCollectionCleared()
        {
            OnPropertyChanged(nameof(Count));
            OnPropertyChanged(_indexerName);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        protected override void InsertItem(int index, KeyValuePair<TKey, TValue> item)
        {
            base.InsertItem(index, item);
            OnCollectionInserted(item, index);
        }

        protected override void RemoveItem(int index)
        {
            var oldItem = this[index];
            base.RemoveItem(index);
            OnCollectionRemoved(oldItem, index);
        }

        protected override void SetItem(int index, KeyValuePair<TKey, TValue> item)
        {
            var oldItem = this[index];
            base.SetItem(index, item);
            OnCollectionSet(item, oldItem, index);
        }

        protected override void ClearItems()
        {
            base.ClearItems();
            OnCollectionCleared();
        }
    }
}
