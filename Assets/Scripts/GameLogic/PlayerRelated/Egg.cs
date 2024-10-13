using Core.Managers;
using Extentions;
using GameLogic.ObstacleGeneration;
using GameLogic.PlayerRelated.GameLogic.PlayerRelated;
using PoolTypes;
using UnityEngine;

namespace GameLogic.PlayerRelated
{
    public class Egg : Bullet
    {

        public override void OnEnable()
        {
            base.OnEnable();
        }

        public override void FixedUpdate()
        {
           base.FixedUpdate();
        }

        public override void OnTriggerEnter2D(Collider2D other)
        {
            ColorBlock colorBlock = other.gameObject.GetComponent<ColorBlock>();
            if (colorBlock is not null && GetColor() == Color.white)
            {
                Color color = colorBlock.GetColor();
                SetColor(color);
            }

            ObstaclePart obstacle = other.gameObject.GetComponent<ObstaclePart>();
            if (obstacle is not null)
            {
                // print($"is different:   {_renderer.color == obstacle.GetColor()}");
                if (UtilityFunctions.CompareColors(GetColor(),obstacle.GetColor()))
                {
                    obstacle.Shatter();
                }
                CoreManager.instance.PoolManager.ReturnToPool(bulletType, gameObject);
            }
        }

        public override void ResetGameObject()
        {
            base.ResetGameObject();
            SetColor(Color.white);
        }
    }
}