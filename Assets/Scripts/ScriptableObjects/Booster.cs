using UnityEngine;
using Core.GameData;
using Core.Managers;
using PoolTypes;
using TMPro;

[CreateAssetMenu(fileName = "Buff", menuName = "TreasureChest/Buff")]
public class Booster : ScriptableObject
{
    public Item boosterType;
    public PoolType poolType;
    public float duration;
    public EventNames activationEvent;
    public EventNames deactivationEvent;
    public UserDataManager.FirebasePath firebasePath; // path to db
}