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
        private float shootingCoolDown;
        private float lastTimeShot;
    
        void Start()
        {
            lastTimeShot = Time.time;
            shootingCoolDown = CoreManager.instance.ControlPanelManager.shootingCooldown;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKey(KeyCode.Space) && Time.time > shootingCoolDown + lastTimeShot)
            {
                
                Shoot();
            }
        }

        public void Shoot()
        {
            GameObject bulletPrefab = CoreManager.instance.PoolManager.GetFromPool(bullet);
            bulletPrefab.transform.position = shootingPosition.position;
            print($"bullet position {bulletPrefab.transform.position}");
            lastTimeShot = Time.time;
            CoreManager.instance.EventManager.InvokeEvent(EventNames.Shoot, null);
        }

        public void TryToShoot()
        {
            if (Time.time > shootingCoolDown + lastTimeShot)
            {
                Shoot();
                print("SHOOOTT");
            }
        }
    }
}
