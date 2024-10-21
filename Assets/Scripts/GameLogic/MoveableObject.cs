using System;
using Core.Managers;
using PoolTypes;
using UnityEngine;

namespace GameLogic
{
    public class MoveableObject : MonoBehaviour
    {
        [SerializeField] private PoolType type;
        private ControlPanelManager controlPanelManager;

        public float MoveSpeed
        {
            get => moveSpeed;
            set => moveSpeed = Mathf.Min(6,value);
        }

        public PoolType PoolType => type;
        
        private float moveSpeed;

        private void Start()
        {
            controlPanelManager = CoreManager.instance.ControlPanelManager;
            // moveSpeed = CoreManager.instance.GameManager.CurrentObjectsSpeed;
            moveSpeed = controlPanelManager.GetGameMoveSpeed();

        }

        public virtual void Update()
        {
            Move();
        }

        public virtual void Move()
        {
            transform.position -= new Vector3(0, CoreManager.instance.ControlPanelManager.GetGameMoveSpeed() * Time.deltaTime, 0);
        }

    }
}