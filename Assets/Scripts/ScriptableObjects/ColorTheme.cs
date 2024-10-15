using Core.GameData;
using Core.Managers;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewColorTheme", menuName = "ColorThemes/ColorTheme", order = 1)]

    public class ColorTheme : ScriptableObject
    {
        public static UserDataManager.FirebasePath path = UserDataManager.FirebasePath.colorThemesOwned;
        
        public Item colorThemeType;
        public Color color1;
        public Color color2;
        public Color color3;
        public Color color4;
        

        public Color[] GetColors()
        {
            return new[] { color1, color2, color3, color4 };
        }
    }
}