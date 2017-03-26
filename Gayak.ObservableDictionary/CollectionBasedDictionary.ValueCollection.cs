using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gayak.Collections
{
    public partial class CollectionBasedDictionary<TKey, TValue>
    {
        public class ValueCollection :
            ICollection<TValue>,
#if !NET40
            IReadOnlyList<TValue>,
#endif
            ICollection
        {
            public ValueCollection(CollectionBasedDictionary<TKey, TValue> source)
            {
                _source = source ?? throw new NullReferenceException(nameof(source));
            }

            private CollectionBasedDictionary<TKey, TValue> _source;

            public TValue this[int index] => _source[index].Value;

            public int Count => _source.Count;

            public bool IsReadOnly => true;

            public object SyncRoot { get; } = new object();

            public bool IsSynchronized => false;

            public void Add(TValue item) => throw new NotSupportedException();

            public bool Remove(TValue item) => throw new NotSupportedException();

            public void Clear() => throw new NotSupportedException();

            public bool Contains(TValue item) => _source
                .Select(x => x.Value)
                .Contains(item);

            public void CopyTo(TValue[] array, int arrayIndex) => _source
                .Select(x => x.Value)
                .ToArray()
                .CopyTo(array, arrayIndex);
            void ICollection.CopyTo(Array array, int index) => this.CopyTo((TValue[])array, index);

            public IEnumerator<TValue> GetEnumerator() => _source.Select(x => x.Value).GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
        }
    }
}
