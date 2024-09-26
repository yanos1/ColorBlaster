using System;
using Core.Managers;
using PoolTypes;
using UnityEngine;

namespace GameLogic
{
    public class MoveableObject : MonoBehaviour
    {
        [SerializeField] private PoolType type;

        public float MoveSpeed
        {
            get => moveSpeed;
            set => moveSpeed = Mathf.Min(6,value);
        }

        public PoolType PoolType => type;
        
        private float moveSpeed;

        private void Start()
        {
            moveSpeed = CoreManager.instance.GameManager.CurrentObjectsSpeed;
        }

        public virtual void OnEnable()
        {
            CoreManager.instance.EventManager.AddListener(EventNames.UpdateObjectMovespeed,IncreaseObjectMovespeed);
        }
        public virtual void OnDisable()
        {
            CoreManager.instance.EventManager.RemoveListener(EventNames.UpdateObjectMovespeed,IncreaseObjectMovespeed);
        }

        private void IncreaseObjectMovespeed(object obj)
        {
            moveSpeed = CoreManager.instance.GameManager.CurrentObjectsSpeed;
        }


        public virtual void Update()
        {
            Move();
        }

        public void Move()
        {
            print(moveSpeed);
            transform.position -= new Vector3(0, moveSpeed * Time.deltaTime, 0);
        }

    }
}