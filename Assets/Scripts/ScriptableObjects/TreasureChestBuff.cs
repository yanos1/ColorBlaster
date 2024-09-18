﻿using System;
using Core.Managers;
using PoolTypes;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameLogic.ConsumablesGeneration
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "Buff", menuName = "TreasureChest/Buff")]
    public class TreasureChestBuff : ScriptableObject
    {
        public BuffType buffType;  // Name for the reward
        public PoolType poolType;  // Icon if needed for UI
        public EventNames activatonEvent;
        public EventNames deactivationEvent;
        public EventNames prefabReachTargetEvent; //will be used for power ups that trigger ui change (like gem pickup)
    }

    public enum BuffType
    {
        None = 0,
        GemBuff = 1,
        ShieldBuff = 2,
        ColorRushBuff = 3,
        
    }
}