using System;
using Core.Managers;
using GameLogic.ObstacleGeneration;
using PoolTypes;
using UnityEngine;

namespace GameLogic.ConsumablesGeneration
{
    public abstract class Consumable : MoveableObject
    {
        public abstract void Consume();

        
    }
}