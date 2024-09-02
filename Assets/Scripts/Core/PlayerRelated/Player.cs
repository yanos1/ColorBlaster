using System;
using Core.Managers;
using Unity.VisualScripting;
using UnityEngine;

namespace Core.PlayerRelated
{
    public class Player : MonoBehaviour
    {
        public Shooter Shooter => shooter;
        public PlayerMovement PlayerMovement => playerMovement;

        [SerializeField] private Shooter shooter;
        [SerializeField] private PlayerMovement playerMovement;

        private static float fallMagnitude => 2f;
        private static float outOfBounds => -8f;

        private Rigidbody2D rb;


        private void Awake()
        {
            CoreManager.instance.Player = this;
            
            rb = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            CoreManager.instance.EventManager.AddListener(EventNames.EndRun, Fall);
            CoreManager.instance.EventManager.AddListener(EventNames.StartGame, ResetGameObject);
        }

        private void OnDisable()
        {
            CoreManager.instance.EventManager.RemoveListener(EventNames.EndRun, Fall);
            CoreManager.instance.EventManager.RemoveListener(EventNames.StartGame, ResetGameObject);
        }

        private void Fall(object obj)
        {
            rb.gravityScale = fallMagnitude;
        }
        

        public void ResetGameObject(object obj)
        {
            rb.gravityScale = 0;
        }

        private void Update()
        {
            if (transform.position.y < outOfBounds)
            {
                CoreManager.instance.EventManager.InvokeEvent(EventNames.GameOver, null);

            }
        }
    }
}