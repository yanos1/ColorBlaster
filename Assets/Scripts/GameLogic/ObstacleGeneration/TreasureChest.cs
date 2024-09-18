using System;
using System.Runtime.CompilerServices;
using Core.Managers;
using Extentions;
using GameLogic.ConsumablesGeneration;
using GameLogic.PlayerRelated;
using PoolTypes;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameLogic.ObstacleGeneration
{
    public class TreasureChest : ObstaclePart
    {
        [SerializeField] private TextMeshProUGUI numberOfShotsUI;
        [SerializeField] private PoolType explodeType;
        [SerializeField] private Transform GemsCollectedUIPosition;

        private int amountOfShotsToBreak;
        private int minAmountOfShots = 2;
        private int maxAmountOfShots = 8;
        private float ySpawnRange = 2;

        private int currentHits;


        public override void Shatter()
        {
            TakeHit();
            if (currentHits == amountOfShotsToBreak)
            {
                CoreManager.instance.BuffManager.ActivateBuff(transform.position, Renderer.color, currentHits);
                // TreasureChestBuff buff = CoreManager.instance.BuffManager.GetReward(Renderer.color);
                // float buffMultiplier = buff.buffType == BuffType.GemBuff ? currentHits * GetGemMultyplier() : 1.5f;
                // CoreManager.instance.BuffManager.MoveParticlesToPlayer(transform.position, buff.poolType,
                //     Renderer.color, buffMultiplier, buff.activatonEvent,buff.deactivationEvent, base.Shatter);
            }
        }

        private int GetGemMultyplier()
        {
            float currentRunTime = CoreManager.instance.TimeManager.GetRunElapsedTime();
            int minMultiplier = Mathf.CeilToInt(currentRunTime / 35);
            int maxMultiplier = Mathf.CeilToInt(currentRunTime / 25);
            int multyplier = (int)Random.Range((float)minMultiplier, maxMultiplier);
            print($" multiplier : {multyplier}");
            return multyplier;
        }

        public override void ResetGameObject()
        {
            base.ResetGameObject();
            amountOfShotsToBreak = Random.Range(minAmountOfShots, maxAmountOfShots);
            numberOfShotsUI.text = amountOfShotsToBreak.ToString();
            currentHits = 0;
            transform.position = new Vector3(transform.position.x, 0, 0); //reset for  y value
            transform.position += Vector3.up * (ySpawnRange * (Random.value >= 0.5 ? 1 : -1));
        }

        private void TakeHit()
        {
            numberOfShotsUI.text = (amountOfShotsToBreak - ++currentHits).ToString();
        }
    }
}