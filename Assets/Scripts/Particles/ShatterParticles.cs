using System.Collections;
using Core.Managers;
using GameLogic.ObstacleGeneration;
using GameLogic.StyleRelated;
using UnityEngine;

namespace Particles
{
    public class ShatterParticles : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] private ParticleSystem particles;
        [SerializeField] private PoolType type;


        public void Init(StyleableObject obj)
        {
            StartCoroutine(PlayParticles(obj));
        }

        private IEnumerator PlayParticles(StyleableObject obj)
        {
            var mainModule = particles.main;
            mainModule.startColor = obj.Renderer.color;
            
            var shape = particles.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.Box;
            shape.scale = obj.Renderer.bounds.size;

            // Position the particle system at the center of the bounds
            particles.transform.position = obj.Renderer.bounds.center;

            particles.Play();
            yield return new WaitForSeconds(particles.main.duration);
            
            CoreManager.instance.PoolManager.ReturnToPool(type, gameObject);
        }
    }
}