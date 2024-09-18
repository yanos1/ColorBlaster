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
        private Dictionary<Color, ValueTuple<ValueTuple<EventNames,EventNames>,float>> activeBuffsDurationsLeft;


        public BuffManager(TreasureChestBuff[] allBuffs)
        {
            colorToBuffMap = new Dictionary<Color, TreasureChestBuff>();
            activeBuffsDurationsLeft = new Dictionary<Color, ((EventNames, EventNames), float)>();
            InitColorToRewardMap(allBuffs);
            CoreManager.instance.EventManager.AddListener(EventNames.EndRun,StopBuffs);
            CoreManager.instance.EventManager.AddListener(EventNames.StartGame,StartUpdate);
        }

        
        public void OnDestroy()
        {
            CoreManager.instance.EventManager.RemoveListener(EventNames.EndRun,StopBuffs);
            CoreManager.instance.EventManager.RemoveListener(EventNames.StartGame,StartUpdate);

        }

        private void StartUpdate(object obj)
        {
            CoreManager.instance.MonoRunner.StartCoroutine(SelfUpdate());

        }
     
        private IEnumerator SelfUpdate()   // can be optimised using events that trigger when a buff is called
        {
            List<Color> keysToRemove = new List<Color>();

            while (CoreManager.instance.GameManager.IsRunActive) 
            {
                keysToRemove.Clear();
                Debug.Log(activeBuffsDurationsLeft.Count);
                foreach (var kvp in activeBuffsDurationsLeft)
                {
                    Debug.Log($"{Time.time}  {kvp.Value.Item2}");
                    if (Time.time > kvp.Value.Item2)
                    {
                        CoreManager.instance.EventManager.InvokeEvent(kvp.Value.Item1.Item2, null);
                        Debug.Log($" invoke this event: {kvp.Value.Item1.Item2.ToString()}");
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
        
        // addbuff(activation, deactivation, duration, 


        private void InitColorToRewardMap(TreasureChestBuff[] allBuffs)
        {
            Color[] currentColors = CoreManager.instance.ColorsManager.CurrentColors;
            int i = 0;
            foreach (var buff in allBuffs)
            {
                colorToBuffMap[currentColors[i++]] = buff;
            }
        }

      

        // i had a problem with floating point.
        // this can be optimised with greater color control (reducing colors to 2 decimal points)
        public TreasureChestBuff GetBuff(Color color)   
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
        
        public void AddBuff(Color color,EventNames activationEvent, EventNames deactivationEvent, float duration)
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
                activeBuffsDurationsLeft[color] = new ValueTuple<ValueTuple<EventNames,EventNames>,float>(
                    (activationEvent, deactivationEvent), expirationTime);
                CoreManager.instance.EventManager.InvokeEvent(activationEvent, color);

            }
        } 


        public void MoveParticlesToPlayer(TreasureChestBuff buff, Vector3 startPosition,Color color, float strength)
        {
            float maxDuration = 0f;
            int numberOfGemsToEarn = (int)strength;
            Transform targetPosition = CoreManager.instance.Player.transform;
            bool firstParticleReached = false;
            for (int i = 0; i < numberOfGemsToEarn; ++i)
            {
                float duration = UnityEngine.Random.Range(0.3f, 0.85f);
                maxDuration = Mathf.Max(duration, maxDuration);

                GameObject gem = CoreManager.instance.PoolManager.GetFromPool(buff.poolType);
                gem.transform.position = startPosition;
                UtilityFunctions.MoveObjectInRandomDirection(gem.transform, 2f);
                gem.GetComponent<SpriteRenderer>().color = color;
                gem.GetComponent<TrailRenderer>().startColor = color;

                CoreManager.instance.MonoRunner.StartCoroutine(UtilityFunctions.MoveObjectOverTime(gem,
                    gem.transform.position, Quaternion.identity,
                    targetPosition, Quaternion.identity, duration, () =>
                    {
                        CoreManager.instance.PoolManager.ReturnToPool(buff.poolType, gem);
                        CoreManager.instance.EventManager.InvokeEvent(buff.prefabReachTargetEvent, null);
                        if (!firstParticleReached)
                        {
                            AddBuff(color,buff.activatonEvent, buff.deactivationEvent, strength);
                            Debug.Log("INVOKE BUFF");
                            firstParticleReached = true;
                        }
                        // TODO: Play earn sound
                    }));
            }
            

        }

        public void StopBuff(Color color)
        {
            foreach (var pair in activeBuffsDurationsLeft)
            {
                if (UtilityFunctions.CompareColors(pair.Key,color))
                {
                    CoreManager.instance.EventManager.InvokeEvent(activeBuffsDurationsLeft[pair.Key].Item1.Item2,null);
                    activeBuffsDurationsLeft.Remove(pair.Key);
                    return;

                } 
            }
        }
        
        private void StopBuffs(object obj)
        {
            foreach (var kvp in activeBuffsDurationsLeft)
            {
                CoreManager.instance.EventManager.InvokeEvent(kvp.Value.Item1.Item2,null); // call stop function
            }
            
            activeBuffsDurationsLeft.Clear();
        }


        public void ActivateBuff(Vector3 startPosition, Color color, float buffStrength)
        {
            TreasureChestBuff buff = GetBuff(color);
            MoveParticlesToPlayer(buff,startPosition,color,buffStrength);
            
        }
    }
}