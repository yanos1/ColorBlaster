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
        public void Shatter()
        {
            // Play the shatter sound *ADD LATER*
            // _audioSource.Play();

            // Instantiate the shatter effect at the part's position
            GameObject shatterEffect = Instantiate(
                CoreManager.instance.StyleManager.GetStyle().ShatterPrefab,
                transform.position,
                Quaternion.identity
            );

            // Set the color of the shatter effect to match the part's color
            var shatterRenderer = shatterEffect.GetComponent<SpriteRenderer>();  // can be optimised
            if (shatterRenderer != null)
            {
                shatterRenderer.color = _renderer.color;
            }

            // Disable the part or handle other shatter logic
            gameObject.SetActive(false);
        }

        public override Style ApplyStyle()
        {
            Style currentStyle = base.ApplyStyle();
            // _audioSource.clip = currentStyle.ShatterSound;
            return null;
        }
        public void ChangeStyle()
        {
            ApplyStyle(); // Reapply the current style when the style changes
        }

        public virtual void ResetGameObject()
        {
            gameObject.SetActive(true);
        }
        
    }
}
