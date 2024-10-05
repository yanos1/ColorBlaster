using Core.Managers;
using UnityEngine;

namespace GameLogic.ObstacleGeneration
{
    public class ChasingObstacleComponent : ObstacleComponent
    {
        private float minDistanceToMove = 0.15f;

        private void Update()
        {
            print("CALLED UPDATEE");
            if (Mathf.Abs(transform.position.x - CoreManager.instance.Player.transform.position.x) > minDistanceToMove)
            {

                transform.position += Vector3.left *
                                      (CoreManager.instance.ControlPanelManager.GetObstacleMovespeed(CoreManager
                                           .instance.ControlPanelManager.chasingObstaclesMovespeed) * Time.deltaTime);
            }
        }
    }
}