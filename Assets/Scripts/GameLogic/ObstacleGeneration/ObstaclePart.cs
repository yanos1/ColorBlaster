using System;
using System.ComponentModel;
using Core.GameData;
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
        private float gemTravelDuration = 0.2f;
        private ObstacleComponent parent;


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

        public virtual void ResetObstacle()
        {
            // Color? deleteColorBuffColor = CoreManager.instance.BoosterManager.IsBuffActive(Item.DeleteColorBuff);
            // if (deleteColorBuffColor is Color color && UtilityFunctions.CompareColors(color, GetColor()))
            // {
            //     return; // we detcted a color that is meant to be inactive, so we return before activating.
            // }

            gameObject.SetActive(true);
        }

        public override void Shatter()
        {
            
            GameObject gem = CoreManager.instance.PoolManager.GetFromPool(PoolType.Gem);
            gem.GetComponent<SpriteRenderer>().color = GetColor();
            gem.GetComponent<TrailRenderer>().startColor = GetColor();
            CoreManager.instance.MonoRunner.StartCoroutine(UtilityFunctions.MoveObjectOverTime(gem, transform.position,
                transform.rotation,
                transform.position +Vector3.up*gemTravelingDistance, transform.rotation, gemTravelDuration,
                () =>
                {
                    CoreManager.instance.EventManager.InvokeEvent(EventNames.GemPrefabArrived, null);
                    CoreManager.instance.PoolManager.ReturnToPool(PoolType.Gem, gem);
                    NotifyParent();
                }));
        
        base.Shatter();
    }

        public void NotifyParent()
        {
            print($"NTIFY PARENT { parent} for {GetInstanceID()}");
            parent.NotifyParent();
        }

        public void SetParent(ObstacleComponent component)
        {
            print($"SET PARENT for {GetInstanceID()}");
            parent = component;
        }
}

}