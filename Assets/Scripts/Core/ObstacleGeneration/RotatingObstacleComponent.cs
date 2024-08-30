using System.Collections;
using System.Collections.Generic;
using Core.ObstacleGeneration;
using ObstacleGeneration;
using UnityEngine;

public class RotatingObstacleComponent : ObstacleComponent
{
    [SerializeField] private Vector3 RotationSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(RotationSpeed * Time.deltaTime);
    }
}
