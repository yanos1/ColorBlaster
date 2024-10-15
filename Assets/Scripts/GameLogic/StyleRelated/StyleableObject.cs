using Core.Managers;
using Particles;
using ScriptableObjects;
using Unity.VisualScripting;
using UnityEngine;

namespace GameLogic.StyleRelated
{
    public abstract class StyleableObject : MonoBehaviour
    {
        public SpriteRenderer Renderer => _renderer;

        private SpriteRenderer _renderer;

        protected AudioSource _audioSource;

        public virtual void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _renderer = GetComponent<SpriteRenderer>();
            if (name.ToString().Contains("Block"))
            {
                print($"CALLED AWAKE ON {name}");
                print($"renderer is {_renderer}");
            }
        }

        public abstract void ChangeStyle();

        public virtual Style ApplyStyle()
        {
            Style currentStyle = CoreManager.instance.StyleManager.GetStyle();

            // Get the current materials of the object
            Material[] currentMaterials = _renderer.sharedMaterials;

            // Check if the object has fewer than 2 materials
            if (currentMaterials.Length < 2)
            {
                // Create a new array with 2 materials
                Material[] newMaterials = new Material[2];

                // Assign the original material to the first slot
                newMaterials[0] = currentMaterials[0];
                // Assign a placeholder or the current style material to the second slot
                newMaterials[1] = currentStyle.Material;

                // Apply the new materials array to the renderer
                _renderer.sharedMaterials = newMaterials;
            }
            // print($"curret style material on {name} : {currentStyle.Material}");

            // Now you can safely modify the second material
            _renderer.sharedMaterials[1] = currentStyle.Material;

            // Apply texture and shader from the style (if needed)
            if (currentStyle.Texture != null)
            {
                _renderer.sharedMaterials[1].mainTexture = currentStyle.Texture;
            }

            if (currentStyle.Shader != null)
            {
                _renderer.sharedMaterials[1].shader = currentStyle.Shader;
            }

            // print($"{name} has {_renderer.sharedMaterials.Length} material elements after assignment");
            // Debug.Log("Styles applied!");

            return currentStyle;
        }


        public virtual void Shatter()
        {
            // Play the shatter sound *ADD LATER*
            // _audioSource.Play();
            // Instantiate the shatter effect at the part's position
            GameObject shaterPrefab = CoreManager.instance.StyleManager.GetShatterPrefab();
            ShapeShiftingParticleSystem particles = shaterPrefab.GetComponent<ShapeShiftingParticleSystem>();
            particles.Init(this);

            // Disable the part or handle other shatter logic
            gameObject.SetActive(false);
        }

        public Color GetColor()
        {
            return _renderer.materials[0].color;
        }


        public virtual void SetColor(Color newColor)
        {
            print($"renderer {_renderer}");
            _renderer.materials[0].color = newColor;
        }
    }
}