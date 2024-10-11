using System;
using System.Collections;
using System.Collections.Generic;
using Core.GameData;
using Core.Managers;
using Extensions;
using Extentions;
using PoolTypes;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace GameLogic.ConsumablesGeneration
{
    public class BoosterManager : MonoBehaviour
    {
        public float ParticleTransferDuration => 0.5f;
        [SerializeField] private Booster[] boosters;

        private Dictionary<Color, Booster> colorToBoosterMap;
        private Dictionary<Color, ValueTuple<Booster, float>> activeBuffsDurationsLeft;


        public void Start()
        {
            colorToBoosterMap = new Dictionary<Color, Booster>();
            activeBuffsDurationsLeft = new Dictionary<Color, (Booster, float)>();
            MapColorToBooster();
            InitBoosters();
        }


        private void OnEnable()
        {
            CoreManager.instance.EventManager.AddListener(EventNames.EndRun, StopBuffs);
            CoreManager.instance.EventManager.AddListener(EventNames.StartGame, StartUpdate);
            CoreManager.instance.EventManager.AddListener(EventNames.FinishedReviving, StartUpdate);
        }


        public void OnDisable()
        {
            CoreManager.instance.EventManager.RemoveListener(EventNames.EndRun, StopBuffs);
            CoreManager.instance.EventManager.RemoveListener(EventNames.StartGame, StartUpdate);
            CoreManager.instance.EventManager.RemoveListener(EventNames.FinishedReviving, StartUpdate);
        }

        private void InitBoosters()
        {
            foreach (var booster in boosters)
            {
                booster.numbersOwned.text = CoreManager.instance.UserDataManager
                    .GetBoosterAmount(booster.boosterType, booster.firebasePath).ToString();
            }
        }

        private void StartUpdate(object obj)
        {
            CoreManager.instance.MonoRunner.StartCoroutine(SelfUpdate());
        }

        private IEnumerator SelfUpdate() // can be optimised using events that trigger when a buff is called
        {
            List<Color> keysToRemove = new List<Color>();

            while (CoreManager.instance.GameManager.IsRunActive)
            {
                keysToRemove.Clear();
                foreach (var kvp in activeBuffsDurationsLeft)
                {
                    // Debug.Log($"{Time.time}  {kvp.Value.Item2}");
                    if (Time.time > kvp.Value.Item2)
                    {
                        CoreManager.instance.EventManager.InvokeEvent(kvp.Value.Item1.deactivationEvent, kvp.Key);
                        Debug.Log($"CANCEL BUFF!");
                        keysToRemove.Add(kvp.Key);
                    }
                }

                // Remove expired buffs
                foreach (var key in keysToRemove)
                {
                    Debug.Log($"REMOVING {key}");
                    activeBuffsDurationsLeft.Remove(key);
                }

                yield return null;
            }
        }

        private void EnableBoosterIfPossible(Booster booster, Color color)
        {
            if (IsBoosterActive(color) ||
                CoreManager.instance.UserDataManager.GetBoosterAmount(booster.boosterType, booster.firebasePath) == 0)
            {
                return;
            }


            CoreManager.instance.UserDataManager.UseBooster(booster.boosterType);
            AddBuff(color, booster);
        }

        private bool IsBoosterActive(Color color)
        {
            return activeBuffsDurationsLeft.ContainsKey(color);
        }
        // addbuff(activation, deactivation, duration, 


        private void MapColorToBooster()
        {
            Color[] currentColors = CoreManager.instance.ColorsManager.AllColors;
            int i = 0;
            foreach (var booster in boosters)
            {
                colorToBoosterMap[currentColors[i]] = booster;
            }
        }


        // i had a problem with floating point.
        // this can be optimised with greater color control (reducing colors to 2 decimal points)
        public Booster GetBuff(Color color)
        {
            foreach (var pair in colorToBoosterMap)
            {
                if (UtilityFunctions.CompareColors(pair.Key, color))
                {
                    // we disable everything but gems power up for test 

                    return colorToBoosterMap[pair.Key];
                }
            }

            return null; // should never reac hehre
        }

        private void AddBuff(Color color, Booster booster)
        {
            if (activeBuffsDurationsLeft.ContainsKey(color))
            {
                Debug.Log("CONTAINED KEY!!!");
                // Update the remaining duration for the existing buff
                var valueTuple = activeBuffsDurationsLeft[color];
                valueTuple.Item2 += booster.duration; // Set the new expiration time
                activeBuffsDurationsLeft[color] = valueTuple;
            }
            else
            {
                Debug.Log("Did not contain key");
                // Set the expiration time as current Time + duration
                float expirationTime = Time.time + booster.duration;
                activeBuffsDurationsLeft[color] = (booster, booster.duration);
                CoreManager.instance.EventManager.InvokeEvent(booster.activatonEvent,
                    (color, booster.duration, GetBuff(color)));
            }
        }


        public void MoveParticlesToBuffUI(Booster booster, Vector3 startPosition, Color color, float strength)
        {
            float maxDuration = 0f;
            int numberOfGemsToEarn = (int)(strength * booster.buffMultiPlier);
            Transform targetPosition = booster.UIButton.transform;
            bool firstParticleReached = false;
            for (int i = 0; i < numberOfGemsToEarn; ++i)
            {
                var duration = ParticleTransferDuration;
                maxDuration = Mathf.Max(duration, maxDuration);

                GameObject gem = CoreManager.instance.PoolManager.GetFromPool(booster.poolType);
                InitPrefab(startPosition, color, gem);

                CoreManager.instance.MonoRunner.StartCoroutine(UtilityFunctions.MoveObjectOverTime(gem,
                    gem.transform.position, Quaternion.identity,
                    targetPosition, Quaternion.identity, duration, () =>
                    {
                        CoreManager.instance.PoolManager.ReturnToPool(booster.poolType, gem);

                        if (!firstParticleReached)
                        {
                            IncrementBoostersOwned(booster, color);
                            firstParticleReached = true;
                        }
                        // TODO: Play earn sound
                    }));
            }
        }

        private static void InitPrefab(Vector3 startPosition, Color color, GameObject gem)
        {
            gem.transform.position = startPosition;
            UtilityFunctions.MoveObjectInRandomDirection(gem.transform, 2f);
            gem.GetComponent<SpriteRenderer>().materials[0].color =
                color; // ew thats ugly, better build a pool that manages allows downcast
            gem.GetComponent<TrailRenderer>().startColor = color;
        }

        private void IncrementBoostersOwned(Booster booster, Color color)
        {
            CoreManager.instance.UserDataManager.AddBooster(booster.boosterType, 1);
            int currentNumber = int.Parse(colorToBoosterMap[color].numbersOwned.text); // Convert string to int
            currentNumber += 1; // Increment the number
            colorToBoosterMap[color].numbersOwned.text = currentNumber.ToString(); // Convert back to string
        }


        public void StopBuff(Color color)
        {
            foreach (var pair in activeBuffsDurationsLeft)
            {
                if (UtilityFunctions.CompareColors(pair.Key, color))
                {
                    CoreManager.instance.EventManager.InvokeEvent(
                        activeBuffsDurationsLeft[pair.Key].Item1.deactivationEvent,
                        color);
                    activeBuffsDurationsLeft.Remove(pair.Key);
                    return;
                }
            }
        }

        private void StopBuffs(object obj)
        {
            foreach (var kvp in activeBuffsDurationsLeft)
            {
                CoreManager.instance.EventManager.InvokeEvent(kvp.Value.Item1.deactivationEvent,
                    kvp.Key); // call stop function
            }

            activeBuffsDurationsLeft.Clear();
        }

        public void AddBuff(Vector3 startPosition, Color color, float buffStrength)
        {
            Booster buff = GetBuff(color);
            MoveParticlesToBuffUI(buff, startPosition, color, buffStrength);
        }

        public Color? IsBuffActive(Item buffType) // can be optimised
        {
            Color buffColor = default;

            foreach (var (color, booster) in colorToBoosterMap)
            {
                if (booster.boosterType == buffType)
                {
                    buffColor = color;
                    foreach (var kvp in activeBuffsDurationsLeft)
                    {
                        if (UtilityFunctions.CompareColors(kvp.Key, buffColor))
                        {
                            return buffColor;
                        }
                    }
                }
            }

            return null;
        }
    }
}