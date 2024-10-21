using System.Collections.Generic;
using Core.Managers;
using Extentions;
using GameLogic.ObstacleGeneration;
using GameLogic.StyleRelated;
using Interfaces;
using PoolTypes;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameLogic.PlayerRelated
{
    namespace GameLogic.PlayerRelated
    {
        public abstract class Bullet : StyleableObject, IResettable
        {

            public float BulletOutOfBoundsYPosition => bulletOutOfBoundsYPosition;
        
            private float bulletOutOfBoundsYPosition = 4.9f;

            [SerializeField] protected PoolType bulletType;
            [SerializeField] private float moveSpeed;
            [SerializeField] private Rigidbody2D rb;
        
        
            public virtual void OnEnable()
            {
                ResetObstacle();
            }

            // Provide a default implementation (can be overridden in child classes)
            public virtual void OnTriggerEnter2D(Collider2D col)
            {
                // Default behavior, which can be empty or overridden by subclasses
            }
        
            public virtual void ResetObstacle()
            {
                gameObject.SetActive(true);
            }
        

            public override Style ApplyStyle()
            {
                Style currentStyle = base.ApplyStyle();
                return currentStyle;
            }

            public virtual void FixedUpdate()
            {
                rb.MovePosition(transform.position + Vector3.up * (Time.deltaTime * moveSpeed));
                if (transform.position.y > bulletOutOfBoundsYPosition)
                {
                    CoreManager.instance.PoolManager.ReturnToPool(bulletType, gameObject);
                }
            }

            public override void ChangeStyle()
            {
                ApplyStyle();
            }
        }
    }

}