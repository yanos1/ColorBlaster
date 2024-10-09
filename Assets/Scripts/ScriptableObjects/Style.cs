using Core.GameData;
using Core.Managers;
using GameLogic.ObstacleGeneration;
using PoolTypes;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewStyle", menuName = "Styles/Style", order = 1)]
    public class Style : ScriptableObject
    {
        public static FirebasePath path = FirebasePath.stylesOwned;

        [SerializeField] private Item styleName;
        [SerializeField] private Material material;
        [SerializeField] private Texture texture;
        [SerializeField] private Shader shader;
        [SerializeField] private PoolType shatterType; // Prefab for shatter effect
        [SerializeField] private AudioClip shatterSound;

        // Public read-only properties to access private fields
        public Item StyleName => styleName;
        public Material Material => material;
        public Texture Texture => texture;
        public Shader Shader => shader;
        public PoolType ShatterType => shatterType;
        public AudioClip ShatterSound => shatterSound;

        // Probably add stats for the volume (to change glow and stuff)

        public Item GetStyle()
        {
            return styleName;
        }
    }
}