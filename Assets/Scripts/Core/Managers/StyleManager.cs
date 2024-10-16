﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.GameData;
using GameLogic.StyleRelated;
using ScriptableObjects;
using UnityEngine;
using static Core.Managers.UserDataManager;

namespace Core.Managers
{

    // public class StyleSaver : ISaveData
    // {
    //     public string StyleName;
    //
    //     public StyleSaver(string styleName)
    //     {
    //         StyleName = styleName;
    //     }
    // }

    public class StyleManager
    {
        // [SerializeField] private Volume _volume;
        private List<StyleableObject> _styleableObjects;
        private Style[] _styles;
        private Style currentStyle; // in the future take this from the saved style for the user


        public StyleManager(Style[] styles)
        {
            _styleableObjects = new List<StyleableObject>();
            _styles = styles;
         

            foreach (var style in _styles)
            {
                if (CoreManager.instance.UserDataManager.IsItemEquipped(style.StyleName, Style.path))
                { 
                    currentStyle = style;
                }
            }
            ApplyStyle(currentStyle.StyleName);
            // // Try to load the saved style, defaulting to Neon if no saved style is found
            // CoreManager.instance.SaveManager.Load<StyleSaver>(savedData =>
            // {
            //     if (savedData != null && Enum.TryParse(savedData.StyleName, out Item savedStyle))
            //     {
            //         currentStyle = _styles.FirstOrDefault(style => style.GetStyle() == savedStyle);
            //     }
            //
            //     // Fallback to default Pastel style if no saved style was found or invalid style saved
            //     if (savedData == null)
            //     {
            //         currentStyle = _styles.FirstOrDefault(style => style.GetStyle() == Item.DefaultStyle);
            //
            //         CoreManager.instance.SaveManager.Save(new StyleSaver(currentStyle.StyleName.ToString()));
            //
            //     }

                // SetStyle(currentStyle.StyleName);
            


        }

        public Style GetStyle()
        {
            return currentStyle;
        }

        public GameObject GetShatterPrefab()
        {
            Debug.Log($"current style {currentStyle}");
            Debug.Log($"current style shatter prefab type {currentStyle.ShatterType}");
            GameObject obj =  CoreManager.instance.PoolManager.GetFromPool(currentStyle.ShatterType);
            Debug.Log($"prefab : {obj}");
            return obj;
        }



        // public void SetStyle(StyleName newStyle)
        // {
        //     Debug.Log("event called!");
        //     CoreManager.instance.SaveManager.Save(new StyleSaver(newStyle.ToString()));
        //     CoreManager.instance.EventManager.InvokeEvent(EventNames.SetStyle, null);
        // }


        public void AddStyleableObject(StyleableObject obj)
        {
            _styleableObjects.Add(obj);
        }

        public void ApplyStyle(Item newStyle)
        {
            currentStyle = _styles.FirstOrDefault(style => style.StyleName == newStyle);
            foreach (var obj in _styleableObjects)
            {
                Debug.Log($"APPLY STYLE FOR {obj.name}");
                obj.ChangeStyle();
            }
        }

        public void RemoveStyleableObject(StyleableObject obj)
        {
            _styleableObjects.Remove(obj);
        }
    }
    
}
