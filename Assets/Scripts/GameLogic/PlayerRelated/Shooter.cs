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

        private TouchInputManager _inputManager;
    
        
        public void Init(TouchInputManager inputManager)
        {
            _inputManager = inputManager;
        }
        void Start()
        {
            lastTimeShot = Time.time;
            shootingCoolDown = CoreManager.instance.ControlPanelManager.shootingCooldown;
            
        }

        // Update is called once per frame
        void Update()
        {
            if (CoreManager.instance.Player.IsDead) return;
            if (Time.time > shootingCoolDown + lastTimeShot && (Input.GetKey(KeyCode.Space) || _inputManager.GetTouchPosition() ==null))
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
        
    }
}
