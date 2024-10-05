using System;
using Core.Managers;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameLogic.ObstacleGeneration
{
    public class ChasingObstacleComponent : ObstacleComponent
    {
        private float minDistanceToMove = 0.15f;
        private float speed;

        private void Start()
        {
            speed = CoreManager.instance.ControlPanelManager.chasingObstaclesMovespeed;
        }

        private void Update()
        {
            if (Mathf.Abs(transform.position.x - CoreManager.instance.Player.transform.position.x) > minDistanceToMove)
            {
                int direction = transform.position.x > CoreManager.instance.Player.transform.position.x ? 1 : -1;

                transform.position += Vector3.left * (CoreManager.instance.ControlPanelManager.GetObstacleSpeedMuliplier(speed) * Time.deltaTime * direction);
            }
        }
    }
}