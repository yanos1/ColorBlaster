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
    
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Shoot();
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
