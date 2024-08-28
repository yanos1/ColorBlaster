using System;
using UnityEngine;

namespace Core.ObstacleGeneration
{
    public class RotatingObstaclePart : ObstaclePart
    {
        
        [SerializeField] private Vector3 rotationSpeed = new Vector3(0, 100, 0); // Rotation speed in degrees per second

        void Update()
        {
            transform.Rotate(rotationSpeed * Time.deltaTime);
        }
    }
}