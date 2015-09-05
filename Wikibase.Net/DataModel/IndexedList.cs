using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wikibase.DataModel
{
    public class IndexedList<K, V> : IEnumerable
    {
        private Dictionary<K, V> dictionary;

        private Func<V, K> indexProvider;

        public int Count { get { return dictionary.Count; } }

        public V this[K key]
        {
            get
            {
                return dictionary[key];
            }
        }

        public IndexedList(Func<V, K> indexProvider, List<V> values = null)
        {
            if (indexProvider == null)
            {
                throw new ArgumentNullException("The index provider must not be null");
            }

            this.dictionary = values == null ? new Dictionary<K, V>() : values.ToDictionary<V, K>(indexProvider);
            this.indexProvider = indexProvider;
        }

        public void Add(V value)
        {
            dictionary.Add(indexProvider(value), value);
        }

        public void Set(V value)
        {
            dictionary[indexProvider(value)] = value;
        }

        public bool ContainsKey(K key)
        {
            return dictionary.ContainsKey(key);
        }

        public bool Remove(K key)
        {
            return dictionary.Remove(key);
        }

        public IEnumerator GetEnumerator()
        {
            return dictionary.Values.GetEnumerator();
        }
    }
}
