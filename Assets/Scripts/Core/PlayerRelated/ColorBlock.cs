using Core.Managers;
using Core.StyleRelated;
using ScriptableObjects;
using UnityEngine;

namespace Core.PlayerRelated
{
    public class ColorBlock : StyleableObject
    {
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
            print("style applied");
        }

      
        
    }
}