using System;
using System.Runtime.CompilerServices;
using Core.Managers;
using Extentions;
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
        private Vector3 playerPosition;

        private int amountOfShotsToBreak;
        private int minAmountOfShots = 2;
        private int maxAmountOfShots = 8;
        private float ySpawnRange = 2;

        private int currentHits;

        private void Start()
        {
            playerPosition = CoreManager.instance.Player.transform.position;
        }

        public override void Shatter()
        {
            TakeHit();
            if (currentHits == amountOfShotsToBreak)
            {
                print(GemsCollectedUIPosition);
                ExplodeAndEarnGems(base.Shatter);
                
            }
        }

        public override void ResetGameObject()
        {
            base.ResetGameObject();
            amountOfShotsToBreak = Random.Range(minAmountOfShots, maxAmountOfShots);
            numberOfShotsUI.text = amountOfShotsToBreak.ToString();
            currentHits = 0;
            transform.position = new Vector3(transform.position.x, 0, 0);  //reset for  y value
            transform.position += Vector3.up * (ySpawnRange * (Random.value >= 0.5 ? 1 : -1));

        }

        private void TakeHit()
        {
            numberOfShotsUI.text = (amountOfShotsToBreak - ++currentHits).ToString();
            
        }

        private void ExplodeAndEarnGems(Action onComplete)
        {
            float maxDuration = 0f;
            int numberOfGemsToEarn = (int)Random.Range(currentHits, (float)currentHits * 3);
            for (int i = 0; i < numberOfGemsToEarn; ++i)
            {
                float duration = Random.Range(0.3f, 0.85f);
                maxDuration = Mathf.Max(duration, maxDuration);

                GameObject gem = CoreManager.instance.PoolManager.GetFromPool(explodeType);
                gem.transform.position = transform.position;
                UtilityFunctions.MoveObjectInRandomDirection(gem.transform,2f);
                gem.GetComponent<SpriteRenderer>().color = Renderer.color;
                gem.GetComponent<TrailRenderer>().startColor = Renderer.color;

                CoreManager.instance.MonoRunner.StartCoroutine(UtilityFunctions.MoveObjectOverTime(gem, gem.transform.position, Quaternion.identity,
                    GemsCollectedUIPosition.position, Quaternion.identity, duration, () =>
                    {
                        CoreManager.instance.PoolManager.ReturnToPool(explodeType, gem);
                        CoreManager.instance.EventManager.InvokeEvent(EventNames.GemPickup, Renderer.color);
                        // TODO: Play earn sound
                    }));
            }
            onComplete.Invoke();

        }
    }
}