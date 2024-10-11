using System;
using Core.GameData;
using Core.Managers;
using PoolTypes;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GameLogic.ConsumablesGeneration
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "Buff", menuName = "TreasureChest/Buff")]
    public class Booster : ScriptableObject
    {
        public Item boosterType;  // Name for the reward
        public PoolType poolType;  // Icon if needed for UI
        public float buffMultiPlier;   // if 5 particles are given, the duration of the buff will be: 5 * buffMultiplier
        public float duration; 
        public EventNames activatonEvent;
        public EventNames deactivationEvent;
        public Button UIButton;   // icon of the buff itself on the buff UI
        public TextMeshProUGUI numbersOwned;
        public UserDataManager.FirebasePath firebasePath;  // path to db


    }
}