 using System;
 using System.Collections;
 using System.Collections.Generic;
 using Core.Managers;
 using Extentions;
 using PoolTypes;
 using UnityEditor;
 using UnityEngine;

namespace GameLogic.ConsumablesGeneration
{
    public class BuffManager
    {
        private Dictionary<Color, TreasureChestBuff> colorToBuffMap;
        private Dictionary<Color, ValueTuple<ValueTuple<Action<Color>,Action>,float>> activeBuffsDurationsLeft;


        public BuffManager(TreasureChestBuff[] allBuffs)
        {
            colorToBuffMap = new Dictionary<Color, TreasureChestBuff>();
            activeBuffsDurationsLeft = new Dictionary<Color, ((Action<Color>, Action), float)>();
            InitColorToRewardMap(allBuffs);
            CoreManager.instance.MonoRunner.StartCoroutine(SelfUpdate());
            CoreManager.instance.EventManager.AddListener(EventNames.EndRun,StopBuffs);
        }

      

        public void OnDestroy()
        {
            CoreManager.instance.EventManager.RemoveListener(EventNames.EndRun,StopBuffs);

        }
        
     
        private IEnumerator SelfUpdate()   // can be optimised using events that trigger when a buff is called
        {
            List<Color> keysToRemove = new List<Color>();

            while (true) 
            {
                keysToRemove.Clear();
                Debug.Log(activeBuffsDurationsLeft.Count);
                foreach (var kvp in activeBuffsDurationsLeft)
                {
                    Debug.Log($"{Time.time}  {kvp.Value.Item2}");
                    if (Time.time > kvp.Value.Item2)
                    {
                        kvp.Value.Item1.Item2.Invoke(); // calling the closing method (to end the buff)
                        keysToRemove.Add(kvp.Key);
                    }
                }

                // Remove expired buffs
                foreach (var key in keysToRemove)
                {
                    Debug.Log("REMOVINGGGGGGGG");
                    activeBuffsDurationsLeft.Remove(key);
                }

                yield return new WaitForSeconds(0.5f); 
            }
        }


        private void InitColorToRewardMap(TreasureChestBuff[] allBuffs)
        {
            Color[] currentColors = CoreManager.instance.ColorsManager.CurrentColors;
            int i = 0;
            foreach (var buff in allBuffs)
            {
                colorToBuffMap[currentColors[i++]] = buff;
            }
        }

      

        public TreasureChestBuff GetReward(Color color)
        {
            foreach (var pair in colorToBuffMap)
            {
                if (UtilityFunctions.CompareColors(pair.Key,color))
                {
                    return colorToBuffMap[pair.Key];

                } 
            }

            return null; // should never reac hehre
        }
        
        public void AddBuff(Color color, ValueTuple<Action<Color>, Action> activationAndDeactivation, float duration)
        {
            
            
            if (activeBuffsDurationsLeft.ContainsKey(color))
            {
                Debug.Log("CONTAINED KEY!!!");
                // Update the remaining duration for the existing buff
                var valueTuple = activeBuffsDurationsLeft[color];
                valueTuple.Item2 +=  duration;  // Set the new expiration time
                activeBuffsDurationsLeft[color] = valueTuple;
            }
            else
            {
                Debug.Log("Did not contain key");
                // Set the expiration time as current Time + duration
                float expirationTime = Time.time + duration;
                activeBuffsDurationsLeft[color] = new ValueTuple<ValueTuple<Action<Color>, Action>, float>(activationAndDeactivation, expirationTime);
                activationAndDeactivation.Item1.Invoke(color); // Activate the buff
            }
        }


        private IEnumerator BuffCoroutine(Color color, Action<Color> activationAction, float duration)
        {
            // Activate the buff
            activationAction?.Invoke(color);

            // Wait for the duration to pass
            yield return new WaitForSeconds(duration);

            // Check if the buff still exists and then deactivate it
            if (activeBuffsDurationsLeft.ContainsKey(color))
            {
                var buff = activeBuffsDurationsLeft[color];
                buff.Item1.Item2?.Invoke(); // Call the deactivation action
                activeBuffsDurationsLeft.Remove(color); // Remove the buff after deactivation
            }
        }


        public void MoveParticlesToPlayer(Vector3 startPosition, PoolType type,Color color, float strength, EventNames reachTargetEvent, Action onComplete)
        {
            float maxDuration = 0f;
            int numberOfGemsToEarn = (int)strength;
            Transform targetPosition = CoreManager.instance.Player.transform;
            for (int i = 0; i < numberOfGemsToEarn; ++i)
            {
                float duration = UnityEngine.Random.Range(0.3f, 0.85f);
                maxDuration = Mathf.Max(duration, maxDuration);

                GameObject gem = CoreManager.instance.PoolManager.GetFromPool(type);
                gem.transform.position = startPosition;
                UtilityFunctions.MoveObjectInRandomDirection(gem.transform, 2f);
                gem.GetComponent<SpriteRenderer>().color = color;
                gem.GetComponent<TrailRenderer>().startColor = color;

                CoreManager.instance.MonoRunner.StartCoroutine(UtilityFunctions.MoveObjectOverTime(gem,
                    gem.transform.position, Quaternion.identity,
                    targetPosition, Quaternion.identity, duration, () =>
                    {
                        CoreManager.instance.PoolManager.ReturnToPool(type, gem);
                        CoreManager.instance.EventManager.InvokeEvent(reachTargetEvent, (color,strength));
                        // TODO: Play earn sound
                    }));
            }
            onComplete?.Invoke();
        }
        
        private void StopBuffs(object obj)
        {
            foreach (var kvp in activeBuffsDurationsLeft)
            {
                kvp.Value.Item1.Item2.Invoke(); // call stop function
            }
            
            activeBuffsDurationsLeft.Clear();
        }

      
    }
}