using System;
using Core.Managers;
using Core.ObstacleGeneration;
using Core.StyleRelated;
using ObstacleGeneration;
using ScriptableObjects;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Core.PlayerRelated
{
    public class Bullet : StyleableObject, Resetable
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private Color baseColor;
        private float outOfScreenPosition = 11f;

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
            transform.Translate(Vector3.right * (Time.deltaTime * moveSpeed));
            if (transform.position.x > outOfScreenPosition)
            {
                CoreManager.instance.PoolManager.ReturnToPool(PoolType.Bullet, gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            ColorBlock colorBlock = other.gameObject.GetComponent<ColorBlock>();
            if (colorBlock is not null)
            {
                Renderer.color = colorBlock.GetColor();
            }

            ObstaclePart obstacle = other.gameObject.GetComponent<ObstaclePart>();
            if (obstacle is not null)
            {
                if (_renderer.color == obstacle.GetColor())
                {
                    obstacle.Shatter();
                }
                else
                {
                    Color[] colors = CoreManager.instance.ColorsManager.CurrentColors;
                    obstacle.SetColor(colors[UnityEngine.Random.Range(0, colors.Length)]);
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