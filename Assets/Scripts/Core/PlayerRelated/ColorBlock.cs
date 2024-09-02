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
        private bool invincible = false;
        private void OnEnable()
        {
            CoreManager.instance.EventManager.AddListener(EventNames.SetStyle, ApplyStyle);
        }
        
        private void OnDisable()
        {
            CoreManager.instance.EventManager.RemoveListener(EventNames.SetStyle, ApplyStyle);
        }

        private void ApplyStyle(object obj)
        {
            Style currentStyle = base.ApplyStyle();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                invincible = !invincible;
            }
            
            
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (invincible) return;
            ObstaclePart part = other.gameObject.GetComponent<ObstaclePart>();
            if (part != null) {
                CoreManager.instance.EventManager.InvokeEvent(EventNames.EndRun, null);
            }
        }
    }
}