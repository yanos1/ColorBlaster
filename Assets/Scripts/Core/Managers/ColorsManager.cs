using System;
using System.Collections.Generic;
using System.Linq;
using Core.GameData;
using Extentions;
using ScriptableObjects;
using UnityEngine;

namespace Core.Managers
{
    public class ColorSaver : ISaveData
    {
        public string SavedColors;

        public ColorSaver(string savedColors)
        {
            SavedColors = savedColors;
        }
    }

    public class ColorsManager
    {
        public Color[] CurrentColors => _currentColorTheme.GetColors()[..maxColors];
        public Color[] AllColors => _currentColorTheme.GetColors();
        
        private  ColorTheme _currentColorTheme;

        // Serialized list where each element holds 4 colors
        private readonly List<ColorTheme> _colorSets;
        private int maxColors;
        private int minAmountOfColors;


        // Dictionary where each enum key maps to an array of 4 colors

        public ColorsManager(List<ColorTheme> colorSets)
        {
            minAmountOfColors = 2;
            _colorSets = colorSets;
            maxColors = minAmountOfColors;
            GetEquippedColorTheme();
            // Load saved color theme
            // CoreManager.instance.SaveManager.Load<ColorSaver>(savedData =>
            // {
            //     // If saved data exists, try to parse it to ColorTheme
            //     if (savedData != null && Enum.TryParse(savedData.SavedColors, out ColorThemeType savedColorTheme))
            //     {
            //         // Try to find the color set from the saved theme
            //         _currentColorTheme = _colorSets.FirstOrDefault(theme => theme.colorThemeType == savedColorTheme)?.GetColors();
            //        
            //
            //     }
            //
            //     // Fallback to default style if no saved style was found or an invalid style was saved
            //     if (_currentColorTheme == null)
            //     {
            //         _currentColorTheme = _colorSets.FirstOrDefault(theme => theme.colorThemeType == ColorThemeType.Default)?.GetColors();
            //         Debug.Log(_currentColorTheme.First());
            //         // Save the default theme if no saved theme was found
            //         CoreManager.instance.SaveManager.Save(new ColorSaver(ColorThemeType.Default.ToString()));
            //     }
            CoreManager.instance.EventManager.AddListener(EventNames.SessionUp, (object obj)=> maxColors=Mathf.Min(maxColors+1,_currentColorTheme.GetColors().Length));
            CoreManager.instance.EventManager.AddListener(EventNames.GameOver, (object obj)=> maxColors=minAmountOfColors);
            CoreManager.instance.EventManager.AddListener(EventNames.StartGame, (object obj) => GetEquippedColorTheme());
        }

        private void GetEquippedColorTheme()
        {
            foreach (var colorSet in _colorSets)
            {
                if (CoreManager.instance.UserDataManager.IsItemEquipped(colorSet.colorThemeType, ColorTheme.path))
                { 
                    _currentColorTheme = colorSet;
                }
            }
        }


        public void OnDestroy()
        {
            CoreManager.instance.EventManager.RemoveListener(EventNames.SessionUp, (object obj)=> maxColors=Mathf.Min(maxColors+1,_currentColorTheme.GetColors().Length));
            CoreManager.instance.EventManager.RemoveListener(EventNames.GameOver, (object obj)=> maxColors=minAmountOfColors);
            CoreManager.instance.EventManager.RemoveListener(EventNames.StartGame, (object obj) => GetEquippedColorTheme());



        }

        public void ChangeColorTheme(Item type)
        {
            _currentColorTheme = _colorSets.FirstOrDefault(theme => type == theme.colorThemeType);
        }

        public Color[] GetThemeColors(Item item)
        {
            return _colorSets.FirstOrDefault(set => set.colorThemeType == item)?.GetColors()[..maxColors];
        }
    }
}