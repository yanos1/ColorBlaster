using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Extentions
{
    public class MonoRunner : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public static void MonoRunnerDontDestroyOnLoad(GameObject obj)
        {
            DontDestroyOnLoad(obj);
        }
        public static GameObject InstantiateObject(GameObject prefab)
        {
            return Instantiate(prefab);
        }

    }
}