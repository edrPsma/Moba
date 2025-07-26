using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Observable
{
    [Serializable]
    public class DictionaryVariable<K, V> : IDictionaryVariable<K, V>
    {
        public V this[K key] => _dictionary[key];
        public bool IsEmpty => _dictionary.Count == 0;
        public IEnumerable<K> Keys => _dictionary.Keys;
        public IEnumerable<V> Values => _dictionary.Values;
        public int Count => _dictionary.Count;
        public bool ContainsKey(K key) => _dictionary.ContainsKey(key);
        public IEnumerator<KeyValuePair<K, V>> GetEnumerator() => _dictionary.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _dictionary.GetEnumerator();

        [SerializeField] protected Dictionary<K, V> _dictionary = new Dictionary<K, V>();
        protected Action<IReadOnlyDictionaryVariable<K, V>> _onModified;

        public void Modifly(Action<Dictionary<K, V>> action)
        {
            action?.Invoke(_dictionary);
            _onModified?.Invoke(this);
        }

        public IUnRegister Register(Action<IReadOnlyDictionaryVariable<K, V>> action, bool runInFirst)
        {
            _onModified += action;
            if (runInFirst)
            {
                action?.Invoke(this);
            }

            return new DictionaryVariableUnRegister<K, V>(this, action);
        }

        public bool TryGetValue(K key, out V value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        public void UnRegister(Action<IReadOnlyDictionaryVariable<K, V>> action)
        {
            _onModified -= action;
        }

        public void UnRegisterAll()
        {
            _onModified = null;
        }
    }
}