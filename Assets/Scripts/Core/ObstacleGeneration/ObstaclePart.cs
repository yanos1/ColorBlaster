using System;
using Core.Managers;
using Core.PlayerRelated;
using Core.StyleRelated;
using ScriptableObjects;
using UnityEngine;

namespace Core.ObstacleGeneration
{
    public class ObstaclePart : StyleableObject, Resetable
    {

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
