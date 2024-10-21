using System;
using Core.Managers;
using GameLogic.ObstacleGeneration;
using GameLogic.StyleRelated;
using UnityEngine;

namespace GameLogic.PlayerRelated
{
    public class Shield : StyleableObject
    {

        [SerializeField] private float opacity;
        private void OnCollisionEnter2D(Collision2D other)
        {
            ObstaclePart obstaclePart = other.gameObject.GetComponent<ObstaclePart>();
            if (obstaclePart is not null)
            {
                //TODO enter sound
                CoreManager.instance.EventManager.InvokeEvent(EventNames.StopBooster, GetColor());
                obstaclePart.Shatter();
                Shatter();
            }
        }

        public override void ChangeStyle()
        {
            base.ApplyStyle();
        }

        public override void ChangeColor(Color color)
        {
            float newAlpha = opacity;
            base.ChangeColor(new Color(color.r, color.g, color.b, newAlpha));
        }
    }
}