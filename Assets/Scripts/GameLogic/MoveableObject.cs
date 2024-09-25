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
        
        public virtual void OnEnable()
        {
            moveSpeed = CoreManager.instance.GameManager.CurrentObjectsSpeed;
        }

        public virtual void Update()
        {
            Move();
        }

        public void Move()
        {
            transform.position -= new Vector3(0, moveSpeed * Time.deltaTime, 0);
        }

    }
}