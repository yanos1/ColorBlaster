using System;
using Core.Managers;
using Core.ObstacleGeneration;
using Core.StyleRelated;
using ScriptableObjects;
using UnityEngine;

namespace Core.PlayerRelated
{
    public class ColorBlock : StyleableObject
    {
        public Vector3 StartingPosition => startingPosition;
        public Vector3 EndGamePosition => endGamePosition;
        public Quaternion StartingRotation => startingRotation;
        
        [SerializeField] private Vector3 endGamePosition;
        
        private Vector3 startingPosition;
        private Quaternion startingRotation;

        private void Start()
        {
            startingPosition = transform.position;
            startingRotation = transform.rotation;
            CoreManager.instance.StyleManager.AddStyleableObject(this);
        }

        private void OnDestroy()
        {
            CoreManager.instance.StyleManager.RemoveStyleableObject(this);
        }

        public override Style ApplyStyle()
        {
            Style currentStyle = base.ApplyStyle();
            return currentStyle;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            ObstaclePart part = other.GetComponent<ObstaclePart>();
            if (part is not null && !CoreManager.instance.Player.IsDead)
            {
                print("INVOKED DEATH");
                CoreManager.instance.EventManager.InvokeEvent(EventNames.KillPlayer, null);
            }
        }

        public override void ChangeStyle()
        {
            ApplyStyle();
        }
    }
}