using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Gayak.Collections
{
    public partial class CollectionBasedDictionary<TKey, TValue>
    {
        public class DictionaryEnumerator : IDictionaryEnumerator, IDisposable
        {
            public DictionaryEnumerator(IEnumerator<KeyValuePair<TKey, TValue>> source)
            {
                _source = source ?? throw new ArgumentNullException(nameof(source));
            }

            private IEnumerator<KeyValuePair<TKey, TValue>> _source;

            public object Key => _source.Current.Key;

            public object Value => _source.Current.Value;

            public DictionaryEntry Entry => new DictionaryEntry(Key, Value);

            public object Current => _source.Current;

            public bool MoveNext() => _source.MoveNext();

            public void Reset() => _source.Reset();

            #region IDisposable Support
            private bool disposedValue = false;

            protected virtual void Dispose(bool disposing)
            {
                if (!disposedValue)
                {
                    if (disposing)
                    {
                        (_source as IDisposable)?.Dispose();
                    }
                    disposedValue = true;
                }
            }

            public void Dispose() => Dispose(true);
            #endregion
        }
    }
}
