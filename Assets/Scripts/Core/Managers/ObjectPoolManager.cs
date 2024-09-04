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
        private PoolEntry[] _entries;
        private Dictionary<PoolType, Queue<GameObject>> pools;
        private Dictionary<PoolType, List<GameObject>> activeObjects = new();

        public ObjectPoolManager(PoolEntry[] entries)
        {
            _entries = entries;
            InitializePool();
            CoreManager.instance.EventManager.AddListener(EventNames.EndRun, ResetPool);
        }

        public void OnDestroy()
        {
            CoreManager.instance.EventManager.RemoveListener(EventNames.EndRun, ResetPool);
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
                activeObjects[entry.type] = new List<GameObject>(); // Initialize activeObjects list for this type
            }
        }

        public GameObject GetFromPool(PoolType type)
        {
            if (pools.TryGetValue(type, out var queue))
            {
                if (queue.Count > 0)
                {
                    GameObject obj = queue.Dequeue();
                    obj.SetActive(true);
                    activeObjects[type].Add(obj);
                    return obj;
                }
                else
                {
                    Debug.LogWarning($"No available objects of type {type} in pool. Consider increasing initial count.");
                }
            }
            else
            {
                Debug.LogError($"Object type {type} not found in pool.");
            }

            return null;
        }

        public void ReturnToPool(PoolType type, GameObject obj)
        {
            if (pools.TryGetValue(type, out var queue))
            {
                obj.SetActive(false);
                queue.Enqueue(obj);

                // Ensure the object is part of the active list before removing it
                if (activeObjects.TryGetValue(type, out var activeList))
                {
                    if (activeList.Contains(obj))
                    {
                        activeList.Remove(obj);
                    }
                    else
                    {
                        Debug.Log($"Returned object of type {type} was not found in active objects list.");
                    }
                }
            }
            else
            {
                Debug.LogError($"Object type {type} not found in pool.");
            }
        }

        public void ResetPool(object obj)
        {
            foreach (var kvp in activeObjects)
            {
                var type = kvp.Key;
                var list = kvp.Value;

                foreach (var item in list.ToArray()) // Use ToArray() to avoid modifying the collection while iterating
                {
                    ReturnToPool(type, item);
                }
            }
        }
    }
}
