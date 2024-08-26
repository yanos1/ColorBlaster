using Core.Managers;
using ScriptableObjects;
using UnityEngine;

namespace Core.StyleRelated
{
    public class StyleableObject : MonoBehaviour
    {
        
        public Renderer Renderer => _renderer;
        
        [SerializeField] protected SpriteRenderer _renderer;
        
        protected AudioSource _audioSource;
        protected Color color;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _audioSource = GetComponent<AudioSource>();
        }
        public virtual Style ApplyStyle()
        {
            Style currentStyle = CoreManager.instance.StyleManager.GetStyle();

            // Apply the material from the style
            _renderer.material = currentStyle.Material;

            // Apply texture and shader from the style (if needed)
            if (currentStyle.Texture != null)
            {
                _renderer.material.mainTexture = currentStyle.Texture;
            }

            if (currentStyle.Shader != null)
            {
                _renderer.material.shader = currentStyle.Shader;
            }

            return currentStyle;
        }
    }
}