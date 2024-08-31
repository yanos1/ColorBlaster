using System;
using System.Collections.Generic;
using Extentions;
using ObstacleGeneration;
using UnityEngine;

namespace Core.Managers
{
    
    [System.Serializable]
    public struct PoolEntry
    {
        public PoolType type;
        public GameObject prefab;
        public int count; // Number of objects to pre-create
    }
    public class ObjectPoolManager 
    {
        private  PoolEntry[] _entries;
        private Dictionary<PoolType, Queue<GameObject>> pools;

        public ObjectPoolManager(PoolEntry[] entries)
        {
            _entries = entries;
            InitializePool();
        }

        private void InitializePool()
        {
            pools = new Dictionary<PoolType, Queue<GameObject>>();

            foreach (PoolEntry entry in _entries)
            {
                Queue<GameObject> objectQueue = new Queue<GameObject>();

                for (int i = 0; i < entry.count; i++)
                {
                    GameObject obj = MonoRunner.InstantiateObject(entry.prefab);
                    MonoRunner.MonoRunnerDontDestroyOnLoad(obj);
                    obj.SetActive(false);
                    objectQueue.Enqueue(obj);
                }

                pools.Add(entry.type, objectQueue);
            }
        }

        public GameObject GetFromPool(PoolType type)
        {
            if (pools.ContainsKey(type))
            {
                if (pools[type].Count > 0)
                {
                    GameObject obj = pools[type].Dequeue();
                    obj.SetActive(true);
                    return obj;
                }
                else
                {
                    Debug.Log($"No available objects of type {type} in pool. Consider increasing initial count.");
                }
            }
            else
            {
                Debug.Log($"Object type {type} not found in pool.");
            }

            return null;
        }

        public void ReturnToPool(PoolType type, GameObject obj)
        {
            if (pools.ContainsKey(type))
            {
                obj.SetActive(false);
                pools[type].Enqueue(obj);
            }
            else
            {
                Debug.LogError($"Object type {type} not found in pool.");
            }
        }
    }
}