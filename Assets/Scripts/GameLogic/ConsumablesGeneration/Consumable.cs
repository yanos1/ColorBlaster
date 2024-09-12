using System;
using Core.Managers;
using GameLogic.ObstacleGeneration;
using PoolTypes;
using UnityEngine;

namespace GameLogic.ConsumablesGeneration
{
    public abstract class Consumable : MonoBehaviour
    {
        public PoolType type;
        public abstract void Consume();

        public void Move()
        {
            if(CoreManager.instance.GameManager.IsGameActive)
            {
                transform.position +=
                    Vector3.left * (CoreManager.instance.GameManager.CurrentObjectsSpeed * Time.deltaTime);
            }
        }

        public virtual void Update()
        {
            Move();
        }
    }
}