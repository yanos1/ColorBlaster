using System;
using System.Collections;
using System.Collections.Generic;
using Core.Managers;
using GameLogic.ConsumablesGeneration;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameLogic.PowerUpGeneration
{
    public class ComsumablesGenerator : MonoBehaviour
    {
        [SerializeField] private List<Consumable> consumables;
        private float nextPowerUpDelay;
        private float maxYPosition = 4f;


        private void Start()
        {
            nextPowerUpDelay = RandomWaitInterval();
            StartCoroutine(SelfUpdate());
        }


        private IEnumerator SelfUpdate()
        {
            while (CoreManager.instance.GameManager.IsGameActive)
            {
                yield return new WaitForSeconds(nextPowerUpDelay);
                GameObject consumable =
                    CoreManager.instance.PoolManager.GetFromPool(consumables[Random.Range(0, consumables.Count)].PoolType);
                GetRandomPosition(consumable);
                nextPowerUpDelay = RandomWaitInterval();
            }
        }

        private void GetRandomPosition(GameObject consumable)
        {
            consumable.transform.position = new Vector3(transform.position.x, Random.Range(-maxYPosition, maxYPosition),0);
        }

        private static int RandomWaitInterval()
        {
            return Random.Range(15,30);
        }
    }
}