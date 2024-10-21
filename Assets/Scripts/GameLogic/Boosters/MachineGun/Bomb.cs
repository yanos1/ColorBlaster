using System.Runtime.CompilerServices;
using Core.Managers;
using GameLogic.ObstacleGeneration;
using GameLogic.PlayerRelated.GameLogic.PlayerRelated;
using PoolTypes;
using UnityEngine;

namespace GameLogic.PlayerRelated
{
    public class Bomb : Bullet
    {
        [SerializeField] private PoolType explosion;

        public override void ResetGameObject()
        {
            base.ResetGameObject();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }
        public override void OnTriggerEnter2D(Collider2D col)
        {
            ObstaclePart obstaclePart = col.GetComponent<ObstaclePart>();
            if (obstaclePart != null)
            {
                obstaclePart.Shatter();
                Explode();
            }
        }

        private void Explode()
        {
            GameObject explosionPrefab = CoreManager.instance.PoolManager.GetFromPool(explosion);
            explosionPrefab.transform.position = transform.position;
            CoreManager.instance.PoolManager.ReturnToPool(bulletType,gameObject);
        }
    }
}