using System;
using UnityEngine;

namespace Extentions
{
    [Serializable]
    public class SerializableTuple <TKey, TValue>: ISerializationCallbackReceiver
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
            _Tuple = new Tuple<TKey, TValue>(key, value);
        }


        public void OnBeforeSerialize()
        {
            return;
        }

        public void OnAfterDeserialize()
        {
            return;
        }
    }
}