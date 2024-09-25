﻿using System.Collections.Generic;
using Core.Managers;
using Extentions;
using GameLogic.ObstacleGeneration;
using GameLogic.StyleRelated;
using Interfaces;
using PoolTypes;
using ScriptableObjects;
using UnityEngine;

namespace GameLogic.PlayerRelated
{
    public class Bullet : StyleableObject, IResettable
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private Color baseColor;
        private float BulletOutOfBoundsYPosition = 10f;

        private void OnEnable()
        {
            ResetGameObject();
        }

        public override Style ApplyStyle()
        {
            Style currentStyle = base.ApplyStyle();
            return currentStyle;
        }

        private void Update()
        {
            transform.Translate(Vector3.up * (Time.deltaTime * moveSpeed));
            if (transform.position.y > BulletOutOfBoundsYPosition)
            {
                CoreManager.instance.PoolManager.ReturnToPool(PoolType.Bullet, gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            ColorBlock colorBlock = other.gameObject.GetComponent<ColorBlock>();
            if (colorBlock is not null)
            {
                Color color = colorBlock.GetColor();
                Renderer.color = color;
                CoreManager.instance.EventManager.InvokeEvent(EventNames.Shoot, color);
            }

            ObstaclePart obstacle = other.gameObject.GetComponent<ObstaclePart>();
            if (obstacle is not null)
            {
                // print($"see difference : {_renderer.color} {obstacle.GetColor()}");
                // print($"is different:   {_renderer.color == obstacle.GetColor()}");
                if (UtilityFunctions.CompareColors(_renderer.color,obstacle.GetColor()))
                {
                    obstacle.Shatter();
                }
                else
                {
                    // List<Color> filteredColors = new List<Color>(CoreManager.instance.ColorsManager.CurrentColors);
                    // filteredColors.Remove(_renderer.color);  // Remove the current color
                    //
                    // obstacle.SetColor(filteredColors[Random.Range(0, filteredColors.Count)]);
                    
                }

                CoreManager.instance.PoolManager.ReturnToPool(PoolType.Bullet, gameObject);
            }
        }

        public void ResetGameObject()
        {
            _renderer.color = baseColor;
        }

        public override void ChangeStyle()
        {
            ApplyStyle();
        }
    }
}