using System;
using System.Collections.Generic;
using ObstacleGeneration;
using UnityEngine;

namespace Core.ObstacleGeneration
{
    public class ObstacleGenerator : MonoBehaviour
    {
        [SerializeField] private ObstacleGeneratorHandler _generatorHandler;

        // Update is called once per frame
        void Update()
        {
            
        }

        public void Init(Dictionary<int, ValueTuple<int , List<Obstacle>>> obstacleData )
        {
            _generatorHandler.Init(obstacleData);
        }
    }
}
