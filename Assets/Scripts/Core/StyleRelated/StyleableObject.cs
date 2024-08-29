using Core.Managers;
using ScriptableObjects;
using UnityEngine;

namespace Core.StyleRelated
{
    public class StyleableObject : MonoBehaviour
    {
        
        public SpriteRenderer Renderer => _renderer;
        
        [SerializeField] protected SpriteRenderer _renderer;
        
        protected AudioSource _audioSource;
        protected Color color;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }
        public virtual Style ApplyStyle()
        {
            Style currentStyle = CoreManager.instance.StyleManager.GetStyle();
            // Apply the material from the style
            _renderer.sharedMaterial = currentStyle.Material;

            // Apply texture and shader from the style (if needed)
            if (currentStyle.Texture != null)
            {
                _renderer.sharedMaterial.mainTexture = currentStyle.Texture;
            }

            if (currentStyle.Shader != null)
            {
                _renderer.sharedMaterial.shader = currentStyle.Shader;
            }

            return currentStyle;
        }
        
        public Color GetColor()
        {
            return color;
        }
<<<<<<< Updated upstream
=======

        public void SetColor(Color newColor)
        {
            color = newColor;
            _renderer.color = color;

        }
>>>>>>> Stashed changes
    }
}