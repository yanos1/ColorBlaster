using Core.Managers;
using GameLogic.ObstacleGeneration;
using UnityEngine;

namespace GameLogic.Consumables
{
    public class Coin : MonoBehaviour,IConsumable
    {
        [SerializeField] private PoolType type;
        public void Consume()
        {
            CoreManager.instance.PoolManager.ReturnToPool(type, gameObject);
            CoreManager.instance.EventManager.InvokeEvent(EventNames.GemPickup, null);
        }
    }
}