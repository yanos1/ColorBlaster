using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.GameData;
using Extentions;
using GameLogic.Boosters;
using UnityEngine;

namespace Core.Managers
{
    public class BoosterManager : MonoBehaviour
    {
        private float particleTransferDuration => 0.5f;


        [SerializeField] private LevelManager levelManager;
        [SerializeField] private BoosterUIManager boosterUI;
        [SerializeField] private BoosterButtonController[] boosterButtonControllers;

        private Dictionary<Color, BoosterButtonController> colorToBoosterMap;
        private Dictionary<Color, ValueTuple<BoosterButtonController, float>> activeBuffsDurationsLeft;

        private int currentActiveBoosters;
        private int nextBoosterToOpenIndex;


        #region Unity Event Functions

        public void Awake()
        {
            colorToBoosterMap = new Dictionary<Color, BoosterButtonController>();
            activeBuffsDurationsLeft = new Dictionary<Color, (BoosterButtonController, float)>();
            currentActiveBoosters = CoreManager.instance.ColorsManager.CurrentColors.Length;
            MapColorToBooster();
            InitBoosters();
        }

        private void OnEnable()
        {
            CoreManager.instance.EventManager.AddListener(EventNames.KillPlayer, StopBuffs);
            CoreManager.instance.EventManager.AddListener(EventNames.StartGame, StartUpdate);
            CoreManager.instance.EventManager.AddListener(EventNames.FinishedReviving, StartUpdate);
            CoreManager.instance.EventManager.AddListener(EventNames.AddBooster, OnAddBooster);
            CoreManager.instance.EventManager.AddListener(EventNames.StopBooster, OnStopBooster);
            CoreManager.instance.EventManager.AddListener(EventNames.SessionUp, OnSessionUp);
            CoreManager.instance.EventManager.AddListener(EventNames.ObstacleCrossed, OnObstacleCroseed);
        }

        private void OnDisable()
        {
            CoreManager.instance.EventManager.RemoveListener(EventNames.KillPlayer, StopBuffs);
            CoreManager.instance.EventManager.RemoveListener(EventNames.StartGame, StartUpdate);
            CoreManager.instance.EventManager.RemoveListener(EventNames.FinishedReviving, StartUpdate);
            CoreManager.instance.EventManager.RemoveListener(EventNames.AddBooster, OnAddBooster);
            CoreManager.instance.EventManager.RemoveListener(EventNames.StopBooster, OnStopBooster);
            CoreManager.instance.EventManager.RemoveListener(EventNames.SessionUp, OnSessionUp);
            CoreManager.instance.EventManager.RemoveListener(EventNames.ObstacleCrossed, OnObstacleCroseed);

        }

        private void OnObstacleCroseed(object obj)
        {
            boosterButtonControllers[nextBoosterToOpenIndex].UpdateObstaclesToOpen();
        }

        private void OnSessionUp(object obj)
        {
            // int start = 0;
            // int end = boosterButtonControllers.Length - 1;  // Fix bounds, use Length - 1
            // int mid;
            //
            // while (start <= end)  // The loop should run while start <= end
            // {
            //     mid = (start + end) / 2;
            //
            //     // Ensure we don't go out of bounds
            //     if (mid > 0 && !boosterButtonControllers[mid].BoosterButton.IsInteractable() &&
            //         boosterButtonControllers[mid - 1].BoosterButton.IsInteractable())
            //     {
            //         boosterButtonControllers[mid].BoosterButton.interactable = true;
            //         return;
            //     }
            //
            //     // Check if we're at the first interactable button transitioning to a non-interactable one
            //     if (mid < boosterButtonControllers.Length - 1 && boosterButtonControllers[mid].BoosterButton.IsInteractable() &&
            //         !boosterButtonControllers[mid + 1].BoosterButton.IsInteractable())
            //     {
            //         boosterButtonControllers[mid + 1].BoosterButton.interactable = true;
            //         return;
            //     }
            //
            //     // Binary search narrowing down based on the mid element's state
            //     if (boosterButtonControllers[mid].BoosterButton.IsInteractable())
            //     {
            //         start = mid + 1;  // Narrow the search to the right
            //     }
            //     else
            //     {
            //         end = mid - 1;  // Narrow the search to the left
            //     }
            // }
            print("NEW SESSION CALLED");
            if (nextBoosterToOpenIndex+1 == boosterButtonControllers.Length)
            {
                CoreManager.instance.EventManager.RemoveListener(EventNames.SessionUp, OnSessionUp);
                
            }
            boosterButtonControllers[nextBoosterToOpenIndex].NumberObstaclesToOpen.gameObject.SetActive(false);
            boosterButtonControllers[nextBoosterToOpenIndex].BoosterButton.interactable = true;
            boosterButtonControllers[++nextBoosterToOpenIndex].UpdateObstaclesToOpen(levelManager.NumObstaclesInSession);
           
        }

        #endregion


        #region Public Functions

        public void OnAddBooster(object obj)
        {
            print("CALL BOOSTER! 222");
            if (obj is (Vector3 startPosition, Color color, int buffStrength))
            {
                print("BOOSTER ACCEPTED 222");
                BoosterButtonController boosterController = GetBooster(color);
                MoveParticlesToBuffUI(boosterController, startPosition, color, buffStrength);
            }
        }

        public Color? IsBuffActive(Item buffType) // can be optimised
        {
            Color buffColor = default;

            foreach (var (color, boosterButtonController) in colorToBoosterMap)
            {
                if (boosterButtonController.Booster.boosterType == buffType)
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

        public void OnStopBooster(object obj)
        {
            if (obj is Color color)
            {
                foreach (var pair in activeBuffsDurationsLeft)
                {
                    if (UtilityFunctions.CompareColors(pair.Key, color))
                    {
                        CoreManager.instance.EventManager.InvokeEvent(
                            activeBuffsDurationsLeft[pair.Key].Item1.Booster.deactivationEvent,
                            color);
                        activeBuffsDurationsLeft.Remove(pair.Key);
                        boosterUI.DeactivateBoosterUI(color);
                        return;
                    }
                }
            }
        }

        #endregion


        #region Private Functions

        private void InitBoosters()
        {
            int index = 0;

            foreach (var boosterButtonController in boosterButtonControllers)
            {
                
                boosterButtonController.NumbersOwnedText.text = CoreManager.instance.UserDataManager
                    .GetBoosterAmount(boosterButtonController.Booster.boosterType,
                        boosterButtonController.Booster.firebasePath).ToString();
                boosterButtonController.BoosterButton.onClick.AddListener(() =>
                    EnableBoosterIfPossible(boosterButtonController,
                        colorToBoosterMap.FirstOrDefault(kvp => kvp.Value == boosterButtonController).Key));
                if (index < currentActiveBoosters)
                {
                    print($"index {index} is interactable");
                    boosterButtonController.BoosterButton.interactable = true;
                }
                else if (index == currentActiveBoosters)
                {
                    print($"index {index} is interactable ???");

                    nextBoosterToOpenIndex = index;
                    boosterButtonController.UpdateObstaclesToOpen(levelManager.NumObstaclesInSession);
                }

                index++;
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
                    if (Time.time > kvp.Value.Item2)
                    {
                        CoreManager.instance.EventManager.InvokeEvent(kvp.Value.Item1.Booster.deactivationEvent,
                            kvp.Key);
                        keysToRemove.Add(kvp.Key);
                    }
                }

                // Remove expired buffs
                foreach (var key in keysToRemove)
                {
                    activeBuffsDurationsLeft.Remove(key);
                }

                yield return null;
            }
        }

        private void EnableBoosterIfPossible(BoosterButtonController boosterButtonController, Color color)
        {
            if (IsBoosterActive(color) ||
                CoreManager.instance.UserDataManager.GetBoosterAmount(boosterButtonController.Booster.boosterType,
                    boosterButtonController.Booster.firebasePath) == 0)
            {
                return;
            }

            print("ENABLE BOOSTER NOW ! 222");
            CoreManager.instance.UserDataManager.UseBooster(boosterButtonController.Booster.boosterType,
                boosterButtonController.UpdateNumbersOwned);
            ActivateBooster(color, boosterButtonController);
        }

        private bool IsBoosterActive(Color color)
        {
            bool active = activeBuffsDurationsLeft.ContainsKey(color);
            Debug.Log($"booster is active on color {color} : {active} 222");
            return active;
        }

        private void MapColorToBooster()
        {
            Color[] currentColors = CoreManager.instance.ColorsManager.AllColors;
            int i = 0;
            foreach (var boosterButtonController in boosterButtonControllers)
            {
                colorToBoosterMap[currentColors[i++]] = boosterButtonController;
            }
        }

        private BoosterButtonController GetBooster(Color color)
        {
            foreach (var pair in colorToBoosterMap)
            {
                if (UtilityFunctions.CompareColors(pair.Key, color))
                {
                    return colorToBoosterMap[pair.Key];
                }
            }

            print("RETURNED NULL  222");

            return null; // should never reach here
        }

        private void ActivateBooster(Color color, BoosterButtonController boosterButtonController)
        {
            if (activeBuffsDurationsLeft.ContainsKey(color))
            {
                // Update the remaining duration for the existing buff
                var valueTuple = activeBuffsDurationsLeft[color];
                valueTuple.Item2 += boosterButtonController.Booster.duration;
                activeBuffsDurationsLeft[color] = valueTuple;
            }
            else
            {
                float expirationTime = Time.time + boosterButtonController.Booster.duration;
                activeBuffsDurationsLeft[color] = (boosterButtonController, expirationTime);
                CoreManager.instance.EventManager.InvokeEvent(boosterButtonController.Booster.activationEvent,
                    (color, boosterButtonController.Booster.duration, GetBooster(color)));
            }

            print("BOOSTER ENABLED 222");
            boosterUI.ActivateBooster(boosterButtonController, color);
        }

        private void MoveParticlesToBuffUI(BoosterButtonController boosterButtonController, Vector3 startPosition,
            Color color, float strength)
        {
            float maxDuration = 0f;
            Transform targetPosition = boosterButtonController.BoosterButton.transform;
            bool firstParticleReached = false;

            for (int i = 0; i < strength; ++i)
            {
                var duration = particleTransferDuration;
                maxDuration = Mathf.Max(duration, maxDuration);

                GameObject gem = CoreManager.instance.PoolManager.GetFromPool(boosterButtonController.Booster.poolType);
                InitPrefab(startPosition, color, gem);

                CoreManager.instance.MonoRunner.StartCoroutine(UtilityFunctions.MoveObjectOverTime(gem,
                    gem.transform.position, Quaternion.identity,
                    targetPosition, Quaternion.identity, duration, () =>
                    {
                        CoreManager.instance.PoolManager.ReturnToPool(boosterButtonController.Booster.poolType, gem);

                        if (!firstParticleReached)
                        {
                            IncrementBoostersOwned(boosterButtonController);
                            firstParticleReached = true;
                        }
                    }));
            }
        }

        private static void InitPrefab(Vector3 startPosition, Color color, GameObject gem)
        {
            gem.transform.position = startPosition;
            UtilityFunctions.MoveObjectInRandomDirection(gem.transform, 2f);
            gem.GetComponent<SpriteRenderer>().materials[0].color = color;
            gem.GetComponent<TrailRenderer>().startColor = color;
        }

        private void IncrementBoostersOwned(BoosterButtonController boosterButtonController)
        {
            CoreManager.instance.UserDataManager.AddBooster(boosterButtonController.Booster.boosterType, 1,
                boosterButtonController.UpdateNumbersOwned);
        }

        private void StopBuffs(object obj)
        {
            foreach (var kvp in activeBuffsDurationsLeft)
            {
                CoreManager.instance.EventManager.InvokeEvent(kvp.Value.Item1.Booster.deactivationEvent,
                    kvp.Key); // call stop function
            }

            activeBuffsDurationsLeft.Clear();

            DisableBoosterButtons();
        }

        private void DisableBoosterButtons()
        {
            foreach (var controller in boosterButtonControllers)
            {
                controller.BoosterButton.interactable = false;
            }
        }

        private void EnableBoosterButtons()
        {
            foreach (var controller in boosterButtonControllers)
            {
                controller.BoosterButton.interactable = true;
            }
        }

        #endregion
    }
}