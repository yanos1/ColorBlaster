using Core.Managers;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewStyle", menuName = "Styles/Style", order = 1)]
    public class Style : ScriptableObject
    {
        [SerializeField] private StyleName styleName;
        [SerializeField] private Color[] colorPalette;
        [SerializeField] private Material material;
        [SerializeField] private Texture texture;
        [SerializeField] private Shader shader;
        [SerializeField] private GameObject shatterPrefab; // Prefab for shatter effect
        [SerializeField] private AudioClip shatterSound;

        // Public read-only properties to access private fields
        public StyleName StyleName => styleName;
        public Color[] ColorPalette => colorPalette;
        public Material Material => material;
        public Texture Texture => texture;
        public Shader Shader => shader;
        public GameObject ShatterPrefab => shatterPrefab;
        public AudioClip ShatterSound => shatterSound;

        // Probably add stats for the volume (to change glow and stuff)

        public StyleName GetStyle()
        {
            return styleName;
        }
    }
}