using System;
using Core.Managers;
using Extentions;
using GameLogic.ConsumablesGeneration;
using GameLogic.StyleRelated;
using Interfaces;
using PoolTypes;
using ScriptableObjects;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace GameLogic.ObstacleGeneration
{
    public class ObstaclePart : StyleableObject, IResettable
    {
        // [SerializeField] private Image gemUIimage;
        private float gemTravelingDistance = 1.2f;


        public override void Awake()
        {
            base.Awake();
        }

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
            if (deleteColorBuffColor is Color color && UtilityFunctions.CompareColors(color, GetColor()))
            {
                return; // we detcted a color that is meant to be inactive, so we return before activating.
            }

            gameObject.SetActive(true);
        }

        public override void Shatter()
        {
            print("gem earned from obstacle");
            float speed = CoreManager.instance.BuffManager.ParticleTransferDuration;
            GameObject gem = CoreManager.instance.PoolManager.GetFromPool(PoolType.Gem);
            gem.GetComponent<SpriteRenderer>().color = GetColor();
            gem.GetComponent<TrailRenderer>().startColor = GetColor();
            CoreManager.instance.MonoRunner.StartCoroutine(UtilityFunctions.MoveObjectOverTime(gem, transform.position,
                transform.rotation,
                transform.position +Vector3.up*gemTravelingDistance, transform.rotation, speed,
                () =>
                {
                    CoreManager.instance.EventManager.InvokeEvent(EventNames.GemPrefabArrived, null);
                    CoreManager.instance.PoolManager.ReturnToPool(PoolType.Gem, gem);
                }));
        
        base.Shatter();
    }
}

}