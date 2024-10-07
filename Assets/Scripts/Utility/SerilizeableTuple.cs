using System;
using System.Collections;
using UnityEngine;

namespace Extensions
{
    [Serializable]
    public class SerializableTuple<TKey, TValue> : ISerializationCallbackReceiver, IEnumerable
    {
        [SerializeField] private TKey _First;
        [SerializeField] private TValue _Second;

        public TKey first => _First;
        
        public TValue second
        {
            get => _Second;
            set => _Second = value;
        }

        private Tuple<TKey, TValue> _Tuple;

        public SerializableTuple(TKey key, TValue value)
        {
            _First = key;
            _Second = value;
            _Tuple = new Tuple<TKey, TValue>(key, value);
        }

        // Empty implementations of serialization callbacks
        public void OnBeforeSerialize()
        {
            return;
        }

        public void OnAfterDeserialize()
        {
            return;
        }

        // Deconstruct method to allow destructuring of the tuple
        public void Deconstruct(out TKey key, out TValue value)
        {
            key = _First;
            value = _Second;
        }

        // IEnumerator implementation to iterate over the two elements of the tuple
        public IEnumerator GetEnumerator()
        {
            yield return _First;  // First item (key)
            yield return _Second; // Second item (value)
        }
    }
}