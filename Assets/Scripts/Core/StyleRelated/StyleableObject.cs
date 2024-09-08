﻿using Core.Managers;
using Particles;
using ScriptableObjects;
using UnityEngine;

namespace Core.StyleRelated
{
    public abstract class StyleableObject : MonoBehaviour
    {
        public SpriteRenderer Renderer => _renderer;

        [SerializeField] protected SpriteRenderer _renderer;

        protected AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public abstract void ChangeStyle();

        public virtual Style ApplyStyle()
        {
            Style currentStyle = CoreManager.instance.StyleManager.GetStyle();
            // Apply the material from the style
            _renderer.sharedMaterial = currentStyle.Material;
            print($"shared material :   {_renderer.sharedMaterial}");
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

        public void Shatter()
        {
            // Play the shatter sound *ADD LATER*
            // _audioSource.Play();
            print("SHATTER");
            // Instantiate the shatter effect at the part's position
            CoreManager.instance.PoolManager.GetFromPool(CoreManager.instance.StyleManager.GetStyle()
                .ShatterType).GetComponent<ShatterParticles>().Init(this);

            // Disable the part or handle other shatter logic
            gameObject.SetActive(false);
        }

        public Color GetColor()
        {
            return _renderer.color;
        }


        public void SetColor(Color newColor)
        {
            _renderer.color = newColor;
        }
        
      
    }
}