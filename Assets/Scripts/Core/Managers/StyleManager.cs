using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ScriptableObjects;
using UnityEngine;

namespace Core.Managers
{
    public class StyleManager : MonoBehaviour
    {
        // [SerializeField] private Volume _volume;
        [SerializeField] private List<Style> styles;
        private Style currentStyle; // in the future take this from the saved style for the user

        private void Start()
        {
            currentStyle = styles.First();
            SetStyle(currentStyle.StyleName);

        }
        
        
        // This is a bad function - CHANGE IT. we need to set the style in a better way
     

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
            CoreManager.instance.EventManager.InvokeEvent(EventNames.SetStyle, null);
            
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