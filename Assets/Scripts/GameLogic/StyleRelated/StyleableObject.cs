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
            // Apply the material from the style
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
            Debug.Log("styles applied !");
            return currentStyle;
        }
        

        public virtual void Shatter()
        {
            // Play the shatter sound *ADD LATER*
            // _audioSource.Play();
            // Instantiate the shatter effect at the part's position
            print(name); 
            
            print(CoreManager.instance.PoolManager);
            GameObject shaterPrefab = CoreManager.instance.StyleManager.GetShatterPrefab();
            print($"shatter prefab {shaterPrefab}");
            ShapeShiftingParticleSystem particles = shaterPrefab.GetComponent<ShapeShiftingParticleSystem>();
            print($"particeles : {particles}");
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