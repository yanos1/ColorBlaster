using Core.Managers;
using GameLogic.ObstacleGeneration;
using PoolTypes;
using UnityEngine;

namespace GameLogic.PlayerRelated
{
    public class Shooter : MonoBehaviour
    {

        [SerializeField] private Transform shootingPosition;
        // Start is called before the first frame update
        private PoolType bullet = PoolType.Bullet;
        private Color lastShotColor;
        private float shootingCoolDown = 0.1f;
        private float lastTimeShot;
    
        void Start()
        {
            lastTimeShot = Time.time;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKey(KeyCode.Space) && Time.time > shootingCoolDown + lastTimeShot)
            {
                
                Shoot();
                lastTimeShot = Time.time;
            }
        }

        public void Shoot()
        {
            GameObject bulletPrefab = CoreManager.instance.PoolManager.GetFromPool(bullet);
            bulletPrefab.transform.position = shootingPosition.position;
            CoreManager.instance.EventManager.InvokeEvent(EventNames.Shoot, null);
        }
    }
}
