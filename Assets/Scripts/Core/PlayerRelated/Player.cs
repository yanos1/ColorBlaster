using System;
using Core.Managers;
using Unity.VisualScripting;
using UnityEngine;

namespace Core.PlayerRelated
{
    public class Player : MonoBehaviour
    {
        private void Awake()
        {
            CoreManager.instance.Player = this;

        }

        public Shooter Shooter => shooter;
        public PlayerMovement PlayerMovement => playerMovement;
        
        [SerializeField] private Shooter shooter;
        [SerializeField] private PlayerMovement playerMovement;

       
    }
}