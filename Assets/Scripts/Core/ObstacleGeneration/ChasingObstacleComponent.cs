using System;
using Core.Managers;
using UnityEngine;

namespace Core.ObstacleGeneration
{
    public class ChasingObstacleComponent : ObstacleComponent
    {
        private float verticalSpeed = 2f;
        private float minDistanceToMove = 0.15f;

        private void Update()
        {
            if (Vector3.Distance(transform.position, CoreManager.instance.Player.transform.position) >
                minDistanceToMove)
            {
                int direction = transform.position.y > CoreManager.instance.Player.transform.position.y ? -1 : 1;

                transform.position += Vector3.up * (verticalSpeed * Time.deltaTime * direction);
            }
        }
    }
}