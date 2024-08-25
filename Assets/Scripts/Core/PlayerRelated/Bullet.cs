using Core.Managers;
using Core.ObstacleGeneration;
using Core.StyleRelated;
using ScriptableObjects;
using UnityEngine;

namespace Core.PlayerRelated
{
    public class Bullet : StyleableObject
    {
        private void OnEnable()
        {
            CoreManager.instance.EventManager.AddListener(EventNames.ChangeStyle, ApplyStyle);
        }
        
        private void OnDisable()
        {
            CoreManager.instance.EventManager.RemoveListener(EventNames.ChangeStyle, ApplyStyle);
        }

        private void ApplyStyle(object obj)
        {
            Style currentStyle = base.ApplyStyle();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            ColorBlock colorBlock = other.gameObject.GetComponent<ColorBlock>();
            if (colorBlock is not null)
            {
                Renderer.material.color = colorBlock.color;
            }

            ObstaclePart obstacle = other.gameObject.GetComponent<ObstaclePart>();
            if (obstacle is not null)
            {
                obstacle.Shatter();
            }
        }
    }
}