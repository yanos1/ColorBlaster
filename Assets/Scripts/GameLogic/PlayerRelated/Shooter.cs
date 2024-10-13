using System;
using Core.Managers;
using GameLogic.Boosters;
using GameLogic.ObstacleGeneration;
using PoolTypes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameLogic.PlayerRelated
{
    public class Shooter : MonoBehaviour
    {

        [SerializeField] private Transform shootingPosition;
        // Start is called before the first frame update
        private PoolType egg = PoolType.Egg;
        private PoolType bomb = PoolType.Bomb;
        private Color savedColor;  // will be used when we shoot bombs 
        private Color lastShotColor;
        private float shootingCoolDown;
        private float lastTimeShot;
        private  PoolType activeBullet;
        
        private TouchInputManager _inputManager;
    
        
        public void Init(TouchInputManager inputManager)
        {
            _inputManager = inputManager;
        }
        void Start()
        {
            lastTimeShot = Time.time;
            shootingCoolDown = CoreManager.instance.ControlPanelManager.shootingCooldown;
            activeBullet = egg;

        }

        private void OnEnable()
        {
            CoreManager.instance.EventManager.AddListener(EventNames.ActivateColorRush, OnActivateColorRush);
            CoreManager.instance.EventManager.AddListener(EventNames.DeactivateColorRush, OnDeactivateColorRush);
        }
        
        private void OnDisable()
        {
            CoreManager.instance.EventManager.RemoveListener(EventNames.ActivateColorRush, OnActivateColorRush);
            CoreManager.instance.EventManager.RemoveListener(EventNames.DeactivateColorRush, OnDeactivateColorRush);
        }

        private void OnDeactivateColorRush(object obj)
        {
            print("DEACTIVATE RUSH");
            activeBullet = egg;
            shootingPosition.position -= Vector3.up;

        }

        private void OnActivateColorRush(object obj)
        {
            print("ACTIVATE RUSH");
            if (obj is (Color color, float duration, BoosterButtonController buff))
            {

                activeBullet = bomb;
                shootingPosition.position += Vector3.up;
                savedColor = color;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (CoreManager.instance.Player.IsDead) return;
            if (_inputManager.Mouse is not null)
            {
                 if (Time.time > shootingCoolDown + lastTimeShot && (_inputManager.GetMousePosition() ==null))
                {
                    Shoot();
                }
            }
            else
            {
                if (Time.time > shootingCoolDown + lastTimeShot && (_inputManager.GetTouchPosition() ==null))
                {
                    Shoot();
                }
            }
           
            
        
        }

        public void Shoot()
        {
            GameObject bulletPrefab = CoreManager.instance.PoolManager.GetFromPool(activeBullet);
            bulletPrefab.transform.position = shootingPosition.position;
            if (activeBullet == bomb)
            {
                bulletPrefab.GetComponent<SpriteRenderer>().sharedMaterials[0].color = savedColor;   // this is bad code
            }
            lastTimeShot = Time.time;
            CoreManager.instance.EventManager.InvokeEvent(EventNames.Shoot, null);
        }
        
    }
}
