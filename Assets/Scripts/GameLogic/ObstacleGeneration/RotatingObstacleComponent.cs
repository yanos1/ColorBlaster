using System;
using Core.Managers;
using UnityEngine;

namespace GameLogic.ObstacleGeneration
{
    public class RotatingObstacleComponent : ObstacleComponent
    {
        // Update is called once per frame
    

        void Update()
        {
            transform.Rotate(new Vector3(0, 0,
               CoreManager.instance.ControlPanelManager.GetObstacleRotationSpeed()) * Time.deltaTime);
        }
    }
}