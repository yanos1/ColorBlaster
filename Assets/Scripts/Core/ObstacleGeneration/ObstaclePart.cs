using System;
using Core.Managers;
using Core.Player;
using Core.PlayerRelated;
using Core.StyleRelated;
using ScriptableObjects;
using UnityEngine;

namespace Core.ObstacleGeneration
{
    public class ObstaclePart : StyleableObject, Resetable
    {
        // public Renderer Renderer => _renderer;
        //
        // private Renderer _renderer;
        // private AudioSource _audioSource;
        // private Color color;
        //
        // private void Awake()
        // {
        //     _renderer = GetComponent<Renderer>();
        //     _audioSource = GetComponent<AudioSource>();
        // }
        //
        // public void ApplyStyle()
        // {
        //     Style currentStyle = CoreManager.instance.StyleManager.GetStyle();
        //
        //     // Apply the material from the style
        //     _renderer.material = currentStyle.Material;
        //
        //     // Apply texture and shader from the style (if needed)
        //     if (currentStyle.Texture != null)
        //     {
        //         _renderer.material.mainTexture = currentStyle.Texture;
        //     }
        //
        //     if (currentStyle.Shader != null)
        //     {
        //         _renderer.material.shader = currentStyle.Shader;
        //     }
        //
        //     // Set the shatter sound from the style
        //     _audioSource.clip = currentStyle.ShatterSound;
        // }

        public void Shatter()
        {
            // Play the shatter sound
            _audioSource.Play();

            // Instantiate the shatter effect at the part's position
            GameObject shatterEffect = Instantiate(
                CoreManager.instance.StyleManager.GetStyle().ShatterPrefab,
                transform.position,
                Quaternion.identity
            );

            // Set the color of the shatter effect to match the part's color
            var shatterRenderer = shatterEffect.GetComponent<Renderer>();  // can be optimised
            if (shatterRenderer != null)
            {
                shatterRenderer.material.color = _renderer.material.color;
            }

            // Disable the part or handle other shatter logic
            gameObject.SetActive(false);
        }

        public override Style ApplyStyle()
        {
            Style currentStyle = base.ApplyStyle();
            _audioSource.clip = currentStyle.ShatterSound;
            return null;
        }
        public void ChangeStyle()
        {
            ApplyStyle(); // Reapply the current style when the style changes
        }

        public void Reset()
        {
            gameObject.SetActive(true);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<Bullet>() is not null)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
