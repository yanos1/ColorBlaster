using System;
using Core.Managers;
using Extentions;
using GameLogic.ConsumablesGeneration;
using GameLogic.StyleRelated;
using Interfaces;
using PoolTypes;
using ScriptableObjects;
using UnityEditor;
using UnityEditor.Playables;
using UnityEngine;

namespace GameLogic.ObstacleGeneration
{
    public class ObstaclePart : StyleableObject, IResettable
    {
     

        public override void ChangeStyle()
        {
            ApplyStyle();
        }
        
        public override Style ApplyStyle()
        {
            Style currentStyle = base.ApplyStyle();
            // _audioSource.clip = currentStyle.ShatterSound;
            return null;
        }

        public virtual void ResetGameObject()
        {
            Color? deleteColorBuffColor = CoreManager.instance.BuffManager.IsBuffActive(BuffType.DeleteColorBuff);
            if (deleteColorBuffColor is not null && Renderer.color == deleteColorBuffColor) return;
            gameObject.SetActive(true);
        }

        public override void Shatter()
        {
            base.Shatter();
            if (CoreManager.instance.BuffManager.IsBuffActive(BuffType.GemBuff) is not null)
            {
                print("gem earned from obstacle");
                float speed = CoreManager.instance.BuffManager.ParticleTransferDuration;
                GameObject gem = CoreManager.instance.PoolManager.GetFromPool(PoolType.Gem);
                gem.GetComponent<SpriteRenderer>().color = Renderer.color;
                gem.GetComponent<TrailRenderer>().startColor = Renderer.color;
                CoreManager.instance.MonoRunner.StartCoroutine(UtilityFunctions.MoveObjectOverTime(gem, transform.position, transform.rotation,
                    CoreManager.instance.Player.transform, transform.rotation, speed,
                    () =>
                    {
                        CoreManager.instance.EventManager.InvokeEvent(EventNames.GemPrefabArrived, null);
                        CoreManager.instance.PoolManager.ReturnToPool(PoolType.Gem, gem);

                    }));
            }
            
            
        }
        
   
        
    }
}
