using System;
using System.Collections;
using System.Collections.Generic;
using Core.Managers;
using PoolTypes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Particles
{
    public class BombExplosion : MonoBehaviour
    {
        [SerializeField] private PoolType explosionParticles;  // Your pool type
        [SerializeField] private ParticleSystem particles;

        private void Start()
        {
            particles = GetComponent<ParticleSystem>();  // Make sure to get the ParticleSystem component
        }

        private void OnEnable()
        {
            particles.Play();
            StartCoroutine(CheckIfParticlesStopped());
        }

        private IEnumerator CheckIfParticlesStopped()
        {
            yield return new WaitUntil(() => particles.isStopped);
            // Particles have finished playing
            CoreManager.instance.PoolManager.ReturnToPool(explosionParticles, gameObject);
            // Perform any action after the particle system ends, like returning to the pool
        }
    }
}
