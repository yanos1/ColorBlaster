using Core.Managers;
using UnityEngine;

namespace GameLogic.ObstacleGeneration
{
    public class ChasingObstacleComponent : ObstacleComponent
    {
        [SerializeField] private float verticalSpeed = 2f;
        private float minDistanceToMove = 0.15f;

        private void Update()
        {
            if (Mathf.Abs(transform.position.x - CoreManager.instance.Player.transform.position.x) > minDistanceToMove)
            {
                int direction = transform.position.x > CoreManager.instance.Player.transform.position.x ? 1 : -1;

                transform.position += Vector3.left * (verticalSpeed * Time.deltaTime * direction);
            }
        }
    }
}