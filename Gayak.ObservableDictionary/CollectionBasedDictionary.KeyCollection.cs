using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gayak.Collections
{
    public partial class CollectionBasedDictionary<TKey, TValue>
    {
        public class KeyCollection :
            ICollection<TKey>,
#if !NET40
            IReadOnlyList<TKey>,
#endif
            ICollection
        {
            public KeyCollection(CollectionBasedDictionary<TKey, TValue> source)
            {
                _source = source ?? throw new NullReferenceException(nameof(source));
            }

            private CollectionBasedDictionary<TKey, TValue> _source;

            public TKey this[int index] => _source[index].Key;

            public int Count => _source.Count;

            public bool IsReadOnly => true;

            public object SyncRoot { get; } = new object();

            public bool IsSynchronized => false;

            public void Add(TKey item) => throw new NotSupportedException();

            public bool Remove(TKey item) => throw new NotSupportedException();

            public void Clear() => throw new NotSupportedException();

            public bool Contains(TKey item) => _source.ContainsKey(item);

            public void CopyTo(TKey[] array, int arrayIndex) => _source
                .Select(x => x.Value)
                .ToArray()
                .CopyTo(array, arrayIndex);
            void ICollection.CopyTo(Array array, int index) => this.CopyTo((TKey[])array, index);

            public IEnumerator<TKey> GetEnumerator() => _source.Select(x => x.Key).GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
        }
    }
}

