using System;
using Core.Managers;
using GameLogic.ConsumablesGeneration;
using GameLogic.ObstacleGeneration;
using ScriptableObjects;
using UnityEngine;

namespace GameLogic.Consumables
{
    public class ColorRushPowerUp : Consumable
    {
        // [SerializeField] private PoolType consumeParticles;
        public override void Consume()
        {
            // CoreManager.instance.PoolManager.GetFromPool(consumeParticles);
            CoreManager.instance.EventManager.InvokeEvent(EventNames.ActivateColorRush,null);
            CoreManager.instance.PoolManager.ReturnToPool(type, gameObject);
        }

        public override void Update()
        {
            base.Update();
        }
    }
}