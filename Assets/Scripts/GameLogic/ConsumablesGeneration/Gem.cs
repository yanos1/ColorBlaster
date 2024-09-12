using Core.Managers;
using GameLogic.ObstacleGeneration;
using PoolTypes;
using UnityEngine;

namespace GameLogic.Consumables
{
    public class Gem : MonoBehaviour,IConsumable
    {
        [SerializeField] private PoolType type;
        public void Consume()
        {
            CoreManager.instance.PoolManager.ReturnToPool(type, gameObject);
            CoreManager.instance.EventManager.InvokeEvent(EventNames.GemPickup, null);
        }
    }
}