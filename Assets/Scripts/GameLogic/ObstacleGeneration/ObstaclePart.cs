using Core.Managers;
using GameLogic.StyleRelated;
using Interfaces;
using ScriptableObjects;
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
            gameObject.SetActive(true);
        }
        
   
        
    }
}
