using System;
using System.Collections.Generic;
using System.Linq;
using Extentions;
using UnityEngine;

namespace Core.Managers
{
    [Serializable] // To make it visible and editable in the Inspector
    public class ColorTheme
    {
        public ColorThemeType type;

        public Color color1;
        public Color color2;
        public Color color3;
        public Color color4;
        
        public Color[] GetColors()
        {
            return new [] {color1,color2,color3, color4};
        }
    }

    public class ColorSaver : ISaveData
    {
        public string SavedColors;

        public ColorSaver(string savedColors)
        {
            SavedColors = savedColors;
        }
    }

    // Define an enum that will be used as keys for the dictionary
    public enum ColorThemeType
    {
        None = 0,
        Default = 1,
        Mystic = 2,
        // Solar =3,
        // Oceanic =4,
        // Cyber =5,
    }

    public class ColorsManager
    {
        public Color[] CurrentColors => _currentColorTheme;
        
        private  Color[] _currentColorTheme;

        // Serialized list where each element holds 4 colors
        private readonly List<ColorTheme> _colorSets;

        // Dictionary where each enum key maps to an array of 4 colors

        public ColorsManager(List<ColorTheme> colorSets)
        {
            _colorSets = colorSets;

            // Load saved color theme
            CoreManager.instance.SaveManager.Load<ColorSaver>(savedData =>
            {
                // If saved data exists, try to parse it to ColorTheme
                if (savedData != null && Enum.TryParse(savedData.SavedColors, out ColorThemeType savedColorTheme))
                {
                    // Try to find the color set from the saved theme
                    _currentColorTheme = _colorSets.FirstOrDefault(theme => theme.type == savedColorTheme)?.GetColors();
                   

                }

                // Fallback to default style if no saved style was found or an invalid style was saved
                if (_currentColorTheme == null)
                {
                    _currentColorTheme = _colorSets.FirstOrDefault(theme => theme.type == ColorThemeType.Default)?.GetColors();
                    Debug.Log(_currentColorTheme.First());
                    // Save the default theme if no saved theme was found
                    CoreManager.instance.SaveManager.Save(new ColorSaver(ColorThemeType.Default.ToString()));
                }
            });
        }

        // You can add methods to get or manipulate colors as needed
        public Color[] GetThemeColors(ColorThemeType colorTheme)
        {
            return _colorSets.FirstOrDefault(theme => theme.type == colorTheme)?.GetColors();
        }

        public void ChangeColorTheme(ColorThemeType type)
        {
            _currentColorTheme = _colorSets.FirstOrDefault(theme => type == theme.type).GetColors();
        }
    }
}