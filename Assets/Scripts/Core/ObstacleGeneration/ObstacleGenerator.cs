using System;
using System.Collections.Generic;
using Core.Managers;
using ObstacleGeneration;
using Unity.VisualScripting;
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

        private void OnTriggerEnter2D(Collider other)
        {
            if (other.gameObject.GetComponent<Obstacle>() is not null)
            {
                Obstacle currentObstacle = _generatorHandler.GetRandomObstacle();
                currentObstacle.transform.position = transform.position;
            }
        }
    }
}
