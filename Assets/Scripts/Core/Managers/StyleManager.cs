using System;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

namespace Core.Managers
{
    public class StyleManager : MonoBehaviour
    {
        // [SerializeField] private Volume _volume;
        [SerializeField] private List<Style> styles;
        private Style currentStyle;

  

        public Style GetStyle()
        {
            return currentStyle;
        }

        public void SetStyle(StyleName newStyle)
        {
            foreach (var style in styles)
            {
                if (style.GetStyle() == newStyle)
                {
                    currentStyle = style;
                }
            }
            CoreManager.instance.EventManager.InvokeEvent(EventNames.ChangeStyle, null);
            
        }
        
    }




    public enum StyleName
    {
        None = 0,
        Neon = 1,
        Glass = 2,
        Pastel = 3,
        Metal = 4
    }

    
}